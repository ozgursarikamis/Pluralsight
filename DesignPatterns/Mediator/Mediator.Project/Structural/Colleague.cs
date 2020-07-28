﻿namespace Mediator.Structural
{
    public abstract class Colleague
    {
        protected Mediator mediator;

        //protected Colleague(Mediator mediator)
        //{
        //    this.mediator = mediator;
        //}
        internal void SetMediator(Mediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual void Send(string message)
        {
            mediator.Send(message, this);
        }

        public abstract void HandleNotification(string message);
    }
}
