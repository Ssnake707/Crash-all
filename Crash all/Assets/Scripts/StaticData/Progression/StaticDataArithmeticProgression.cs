using UnityEngine;

namespace StaticData.Progression
{
    [CreateAssetMenu(fileName = "ArithmeticProgression", menuName = "Static data/Progression", order = 0)]
    public class StaticDataArithmeticProgression : BaseProgression
    {
        //  начальное значение
        public float InitialValue;
        // разница между значениями на соседних уровнях
        public float ProgressionStep;
        
        public override float GetValue(int level) => 
            InitialValue + (level - 1) * ProgressionStep;
    }
}