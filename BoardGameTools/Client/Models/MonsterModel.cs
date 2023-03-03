using BoardGameTools.Shared.Models;

namespace BoardGameTools.Client.Models;

public class MonsterModel
{
    public long Id { get; }

    public string Name { get; }

    public long Attack { get; }

    public long Armor { get; }

    public string DefensiveAbilities { get; }

    public string OffensiveAbilities { get; }

    public MonsterModel(long id, string name, long attack, long armor, string defensiveAbilities, string offensiveAbilities)
    {
        Id = id;
        Name = name;
        Attack = attack;
        Armor = armor;
        DefensiveAbilities = defensiveAbilities;
        OffensiveAbilities = offensiveAbilities;
    }

    public static List<MonsterModel> Transform(List<Monster> monsters)
    {
        return monsters.Select(monster =>
        {
            var defensive = monster.DefensiveAbilities == null 
                ? string.Empty
                : ((DefensiveAbilities)monster.DefensiveAbilities).ToString();

            var offensive = monster.OffensiveAbilities == null 
                ? string.Empty
                : ((OffensiveAbilities)monster.OffensiveAbilities).ToString();

            return new MonsterModel(
                monster.Id,
                monster.Name,
                monster.Attack,
                monster.Armor,
                defensive,
                offensive
                );
        }).ToList();
    }

    public static List<Monster> Transform(List<MonsterModel> monsters)
    {
        return monsters.Select(monster => new Monster()
        {
            Id = monster.Id,
            Name = monster.Name,
            Attack = monster.Attack,
            Armor = monster.Armor,
            DefensiveAbilities = string.IsNullOrEmpty(monster.DefensiveAbilities) 
                ? null 
                : (int)Enum.Parse(typeof(DefensiveAbilities), monster.DefensiveAbilities),
            OffensiveAbilities = string.IsNullOrEmpty(monster.OffensiveAbilities) 
                ? null 
                : (int)Enum.Parse(typeof(OffensiveAbilities), monster.OffensiveAbilities)
        }).ToList();
    }
}