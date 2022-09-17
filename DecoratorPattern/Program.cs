using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorPattern
{
    public interface IMailService
    {
        bool SendMail(string message);
    }
    //concreteComponenet1 
    public class CloudMailService : IMailService
    {
        public bool SendMail(string message)
        {
            Console.WriteLine($"Mail send {message} via {nameof(CloudMailService)}");
            return true;
        }
    }
    //concreteComponent2 
    public class OnPremiseMailSend : IMailService
    {
        public bool SendMail( string message)
        {
            Console.WriteLine($"Mail Send Message {message} via {nameof(OnPremiseMailSend)}");
            return true;
        }
    }

    //Decorator 
    public abstract class MailServiceDecoratorBase : IMailService
    {
        private readonly IMailService _iMailService;
        public MailServiceDecoratorBase(IMailService mailService)
        {
            _iMailService = mailService;
        }

        public virtual bool SendMail( string message)
        {
            return _iMailService.SendMail(message);
        }
    }
    //concrete Decorator
    public class StatisticsDecorator : MailServiceDecoratorBase
    {
        public StatisticsDecorator(IMailService mailservice) : base(mailservice)
        {

        }
        public override bool SendMail(string message)
        {
            //Fake Collecting statistics
            Console.WriteLine($"Collecting statistics in {nameof(StatisticsDecorator)}");
            return base.SendMail(message);

        }
    }
        public class MessageDatabaseDecorator : MailServiceDecoratorBase
        {
            public List<string> SendMessage { get; private set; } = new List<string>();
            public MessageDatabaseDecorator(IMailService mailservice):base(mailservice)
            {

            }
            public override bool SendMail(string message)
            {
                if(base.SendMail(message))
                {
                    SendMessage.Add(message);
                    return true;
                }
                return false;
            }
        }
    
    class Program
    {
        static void Main(string[] args)
        {
            var cloudMailService = new CloudMailService();
            cloudMailService.SendMail("hi, there");

            var onPremiseService = new OnPremiseMailSend();
            onPremiseService.SendMail("hello, there");

            //now Decorate with additional service 
            var statisticsDecorator = new StatisticsDecorator(cloudMailService);
            statisticsDecorator.SendMail($"hi, there via {nameof(StatisticsDecorator)} Wrapper");

            // now Decorate StoreMessage decorator
            var _messageBaseDecoratory = new MessageDatabaseDecorator(onPremiseService);
            _messageBaseDecoratory.SendMail($" hi there, sending mail via {nameof(MessageDatabaseDecorator)} wrapper, message1");

            _messageBaseDecoratory.SendMail($"Sending mail agian via {nameof(MessageDatabaseDecorator)} wrapper, message 2");

            foreach( var message in _messageBaseDecoratory.SendMessage)
            {
                Console.WriteLine($"Stored Message : {message}");
            }
            Console.ReadKey();

         }
    }
}
