using System.Collections.Generic;
using UnityEngine;
using YGFIL.Monsters;

namespace YGFIL.Databases 
{
    public class NamesDatabase 
    {
        public static Dictionary<MonsterType, string> Names = new Dictionary<MonsterType, string>() 
        {
            {MonsterType.Zombie, "Pedro Podrido"},
            {MonsterType.MainCharacter, "Tú"},
            {MonsterType.Caster, ""},
            {MonsterType.Medusa, "Carolina Culebras"},
            {MonsterType.Vampire, "Conde Chupón"},
            {MonsterType.Spider, "Telma Telaraña"},
            {MonsterType.Wolf, "LisandroLicántropo"},
            {MonsterType.Succubus, "Penélope Pechugona"}
        };
    }
}