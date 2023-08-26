using UnityEngine;

namespace StaticData.Progression
{
    // Арифметическая прогрессия предполагает, что каждый следующий уровень или этап
    // требует постоянного прироста (например, увеличение опыта или ресурсов на постоянное значение).
    [CreateAssetMenu(fileName = "ArithmeticProgression", menuName = "Static data/Progression/Arithmetic Progression", order = 0)]
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