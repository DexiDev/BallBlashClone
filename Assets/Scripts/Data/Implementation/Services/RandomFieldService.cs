using DexiDev.Game.Data.Fields;

namespace DexiDev.Game.Data.Services
{
    public class RandomFieldService<T> where T : IDataField
    {
        protected T GetRandomField(RandomField<T> randomField)
        {
            float totalChance = 0f;
            foreach (var randomData in randomField.Value)
            {
                totalChance += randomData.Chance;
            }

            float randomValue = UnityEngine.Random.Range(0f, totalChance);

            float cumulativeChance = 0f;
                
            foreach (var rewardRandomData in randomField.Value)
            {
                cumulativeChance += rewardRandomData.Chance;
                if (randomValue <= cumulativeChance)
                {
                    return rewardRandomData.Field;
                }
            }

            return default;
        }
    }
}