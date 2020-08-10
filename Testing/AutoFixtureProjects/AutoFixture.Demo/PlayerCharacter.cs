using System.ComponentModel.DataAnnotations;

namespace AutoFixtureDemo
{
    public class PlayerCharacter
    {
        [StringLength(20)]
        public string RealName { get; set; }

        [StringLength(8)]
        public string GameCharacterName { get; set; }

        [Range(0, 50)]
        public int CurrentHealth { get; set; }
    }
}
