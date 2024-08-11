using UserManagementAPI.DataAccessLayer.Repository;
using UserManagementAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.DataAccessLayer;
using UserManagementAPI.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace UserManagementAPI.BusinessLayer.Service
{
 

    namespace UserManagementAPI.BusinessLayer.Service
    {
        public class UserService : IUserService
        {
            private readonly IRepository<User> _userRepository;
            private readonly IRepository<AuditLog> _auditLogRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserRepository _iuserRepository;

            // L'iniezione delle dipendenze viene effettuata qui tramite il costruttore
            public UserService(IRepository<User> userRepository, IRepository<AuditLog> auditLogRepository, IUnitOfWork unitOfWork, IUserRepository iuserRepository)
            {
                _userRepository = userRepository;             
                _auditLogRepository = auditLogRepository;
                _unitOfWork = unitOfWork;
                _iuserRepository = iuserRepository;
            }

            public async Task<IEnumerable<User>> GetAllUsersAsync()
            {
                return await _userRepository.GetAllAsync();
            }

            public async Task<User> GetUserByIdAsync(int id)
            {
                return await _userRepository.GetByIdAsync(id);
            }

            public async Task AddUserAsync(User user)
            {
                try
                {
                    var operation = "Creazione utente";
                    var timestamp = DateTime.Now;

                    await _iuserRepository.ExecuteAddUserWithLogAsync(user, operation, timestamp);
                }
                catch (Exception ex)
                {
                    throw new Exception("Operazione fallita", ex);
                }
            }

            // Update con gestione dell'islamento della transizione da .Net

            public async Task UpdateUserAsync(User user)
            {
                // Avvia una transazione con il livello di isolamento Serializable
                _unitOfWork.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    // Aggiorna l'utente nel database
                    _userRepository.Update(user);

                    // Crea un log per registrare l'azione
                    var log = new AuditLog
                    {
                        UserId = user.Id,
                        Operation = "Utente modificato con successo",
                        Timestamp = DateTime.Now
                    };

                    // Inserisci il log nel contesto
                    await _auditLogRepository.InsertAsync(log);

                    // Salva tutte le modifiche fatte (utente aggiornato e log inserito) in una volta
                    await _unitOfWork.CompleteAsync();

                    // Conferma la transazione se non ci sono stati problemi
                    _unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();  
                    throw new Exception("Operazione fallita", ex);
                }
            }

            public async Task DeleteUserAsync(int id)
            {
                // Salva l'ID dell'utente in una variabile temporanea
                var userId = id;

                // Registra il log al di fuori della transazione principale
                var log = new AuditLog
                {
                    UserId = userId,
                    Operation = "Cancellazione avvenuta con successo",
                    Timestamp = DateTime.Now
                };

                try
                {
                    await _auditLogRepository.InsertAsync(log);
                    await _unitOfWork.CompleteAsync();  // Salva il log

                    // Effettua la cancellazione dell'utente
                    await DeleteUserInternalAsync(userId);
                }
                catch (Exception ex)
                {
                    // Se la cancellazione fallisce, elimina il log creato in precedenza
                    if (log.AuditLogId != 0)
                    {
                        _auditLogRepository.Delete(log.AuditLogId);
                        await _unitOfWork.CompleteAsync();  // Salva l'eliminazione del log
                    }

                    throw new Exception("Cancellazione fallita", ex);
                }
            }

            private async Task DeleteUserInternalAsync(int userId)
            {
                // Inizia la transazione principale
                _unitOfWork.BeginTransaction();

                try
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null)
                    {
                        throw new Exception("Utente non trovato");
                    }

                    // Elimina l'utente
                    _userRepository.Delete(userId);
                    await _unitOfWork.CompleteAsync();  // Salva la cancellazione

                    // Commit della transazione
                    _unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    throw new Exception("Cancellazione fallita", ex);
                }
            }


        }
    }

}
