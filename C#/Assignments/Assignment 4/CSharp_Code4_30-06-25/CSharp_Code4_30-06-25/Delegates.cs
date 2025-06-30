using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lists
{
    public delegate void RingEventHandler(string message);
    internal class MobilePhone
    {

        public event RingEventHandler OnRing;
        public void ReceiveCall()
        {
            if (OnRing != null)
            {
                OnRing("Incoming Call");
            }
        }
    }

    class Subscriber
    {
        public void PlayRingtone(string message)
        {
            Console.WriteLine("Playing Ringtone" + message);
        }

        public void CallerInfo(string message)
        {
            Console.WriteLine("Displaying caller information" + message);
        }
        public void Vibrate(string message)
        {
            Console.WriteLine("Phone is vibrating" + message);
        }


    }
    class EventDemo1
    {
        static void Main()
        {
            MobilePhone mob = new MobilePhone();
            Subscriber subscriber = new Subscriber();


            mob.OnRing += subscriber.PlayRingtone;
            mob.OnRing += subscriber.CallerInfo;
            mob.OnRing += subscriber.Vibrate;
            mob.ReceiveCall();


            Console.ReadLine();
        }
    }
}
