using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatRDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediatRDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator) => this._mediator = mediator;

        [HttpGet("{id}")]
        public async Task<Contact> GetContact([FromRoute] ContactRequest contactRequest)
        {
            return await _mediator.Send(contactRequest);
        }

        #region Nested Classes

        public class ContactRequest : IRequest<Contact>
        {
            public int Id { get; set; }
        }

        public class ContactRequestHandler : IRequestHandler<ContactRequest, Contact>
        {
            private readonly ContactsContext _db;

            public ContactRequestHandler(ContactsContext db) => _db = db;

            public Task<Contact> Handle(ContactRequest request, CancellationToken cancellationToken)
            {
                return _db.Contacts.Where(c => c.Id == request.Id)
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }
        #endregion
    }
}
