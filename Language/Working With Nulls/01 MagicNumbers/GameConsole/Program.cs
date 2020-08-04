using System;

namespace GameConsole
{
    internal class Program
    {
        private static void Main()
        {
            var sarah = new PlayerCharacter(new DiamondSkinDefence())
            {
                Name = "Sarah"
            };
            var amrit = new PlayerCharacter(SpecialDefence.Null)
            {
                Name = "Amrit"
            };
            var gentry = new PlayerCharacter(SpecialDefence.Null)
            {
                Name = "Gentry"
            };

            sarah.Hit(10);
            amrit.Hit(10);
            gentry.Hit(10);

            Console.ReadLine();
        }
    }

    //public class NullDefence : SpecialDefence
    //{
    //    public override int CalculateDamageReduction(int totalDamage)
    //    {
    //        return 0; // no operation / 'do nothing' behaviour
    //    }
    //}

    public abstract class SpecialDefence
    {
        public abstract int CalculateDamageReduction(int totalDamage);
        public static SpecialDefence Null { get; } = new NullDefence();

        public class NullDefence : SpecialDefence
        {
            // all developers should agree on what default action should be:
            public override int CalculateDamageReduction(int totalDamage)
            {
                return 0; // no operation / 'do nothing' behaviour
            }
        }
    }

    public interface ISpecialDefence
    {
        int CalculateDamageReduction(int totalDamage);
    }

    public class IronBonesDefence : SpecialDefence
    {
        public override int CalculateDamageReduction(int totalDamage)
        {
            return 5;
        }
    }
    public class DiamondSkinDefence : SpecialDefence
    {
        public override int CalculateDamageReduction(int totalDamage)
        {
            return 1;
        }
    }

    public class PlayerCharacter
    {
        private readonly SpecialDefence _specialDefence;

        public PlayerCharacter(SpecialDefence specialDefence)
        {
            _specialDefence = specialDefence;
        }

        public string Name { get; set; }
        public int Health { get; set; } = 100;

        public void Hit(int damage)
        {
            //int damageReduction = 0;
            //if (_specialDefence != null)
            //{
            //    damageReduction = _specialDefence.CalculateDamageReduction(damage);
            //}

            //var totalDamageTaken = damage - damageReduction;

            int totalDamageTaken = damage - _specialDefence.CalculateDamageReduction(damage);
            Health -= totalDamageTaken;
            Console.WriteLine($"{Name}'s health has been reduced by {totalDamageTaken} to {Health}");
        }
    }

}
