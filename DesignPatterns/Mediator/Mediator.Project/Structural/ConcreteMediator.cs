using System.Collections.Generic;
using System.Linq;

namespace Mediator.Structural
{
    public class ConcreteMediator: Mediator
    { 
        //public Colleague Colleague1 { get; set; }
        //public Colleague Colleague2 { get; set; }
        private readonly List<Colleague> colleagues = new List<Colleague>();

        public void Register(Colleague colleague)
        {
            // bi-directional references:
            colleague.SetMediator(this);
            this.colleagues.Add(colleague);
        }
        public override void Send(string message, Colleague colleague)
        {
            //if (colleague == Colleague1)
            //{
            //    Colleague2.HandleNotification(message);
            //}
            //else
            //{
            //    Colleague1.HandleNotification(message);
            //}
            colleagues.Where(x => x != colleague).ToList()
                .ForEach(c => c.HandleNotification(message));
        }
    }
}