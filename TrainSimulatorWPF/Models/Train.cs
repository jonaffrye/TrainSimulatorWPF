using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainSimulator
{
    public class Train
    {
        public string Type { get; private set; } //IC, P, L, S
        public string Name { get; private set; }

        private double MaxSpeed;
        public double NormalSpeed { get; private set; }
        private DateTime CreationDate;
        private int PlaceNbr;
        private int Width;
        private int Length;
        private int CoachNbr;


        public double Mass { get; set; }
        public double MaxTractionForce { get; set; }
        public double MaxBrakeForce { get; set; }
        public List<double> ResCoef { get; set; }
        public double Velocity { get; private set;}
        public double Position { get; private set; }

        //Cabin TrainCabin; 
        public Train(string name, double normalSpeed)
        {
            Name = name;
            NormalSpeed = normalSpeed;
            Velocity = 0;
            Position = 0; 
        }
        public Train(string type, string name, double maxSpeed, double normalSped, DateTime creationDate, 
            int placeNbr, int width, int length, double mass, double maxTractionFroce, double maxBrakeForce, List<double> resCoef)
        {
            Type = type;
            Name = name;
            MaxSpeed = maxSpeed;
            NormalSpeed = normalSped;
            CreationDate = creationDate;
            PlaceNbr = placeNbr;
            Width = width;
            Length = length;
            Velocity = 0;
            Position = 0;
            Mass = mass;
            MaxTractionForce = maxTractionFroce;
            MaxBrakeForce = maxBrakeForce;
            ResCoef = resCoef; 
        }


        public void StartTrain()
        {
            // check if the start procedure is valid
            Console.WriteLine("The train has started\r\n"); 
        }
 
        public void StopTrain()
        {
            Console.WriteLine("The train is stopped\r\n");
        }

        /* model : 
         *  sum(F) = ma
         *  a = (F_thr - F_brk - F_res)/m
         *  F_thr = thrVal*F_max_thr 
         *  F_brk = brkVal*F_max_brk
         *  F_res = c1 + c2*v + c3*v^2 (Davis's eq)
         *  a = dv/dt => dv = a*dt + v_t
         *  v = dx/dt => dx = v*dt + x_t
         * */
        public void UpdateVelocity(double timeDT, double commandValue)
        {
            double brakeValue, throttleValue;
            // since same slider is used for both throttle and brake
            brakeValue = throttleValue = commandValue;

            double ResForce = ResCoef[0] + ResCoef[1] * Velocity + (ResCoef[2] * Velocity * Velocity);
            double Acceleration = (throttleValue * MaxTractionForce - brakeValue * MaxBrakeForce - ResForce) / Mass ;
            Velocity = Acceleration * timeDT + Velocity;
            if (Velocity < 0) Velocity = 0; // avoid the train to back up
        }

        public void UpdatePosition(double timeDT)
        {
            Position = Velocity * timeDT + Position; 
        }

        public override string ToString()
        {
            return Name + "\nType: " + Type + " " + "\nWeight: "+Mass*9.81+ "Kg\nMaxSpeed: " + 
                MaxSpeed + "km\nAverage speed: " + NormalSpeed + "km\nCreation date: " + CreationDate + 
                "\nNumber of seats: " + PlaceNbr + " places\nLength: " + Length + "m\n"; 
        } 

    }
}
