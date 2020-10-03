using System.Collections;
using Cubipool.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Repository.Abstractions
{
	public interface IRequestRepository
	{
		Task<Request> CreateOneAsync(Request request);
		Task<IEnumerable<Request>> GetAllWaitingRequestsByReservationId(int id);
		IEnumerable<Request> GetAllPendingAndAcceptedCurrentRequestsByUserId(int id);
		Task<Request> UpdateOneAsync(Request request);
		Task<Request> GetOneByIdAsync(int id);
		IEnumerable<Request> GetAllByUserIdAndPublicationId(int userId, int publicationId);
		Task<IEnumerable<Request>> GetAllByListIdAsync(IEnumerable<int> ids);
		Task<ICollection<Request>> GetAllPendingAndAcceptedRequestByPublicationId(int id);
	}
}