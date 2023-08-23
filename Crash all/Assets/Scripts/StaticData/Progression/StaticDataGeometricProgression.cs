using UnityEngine;

namespace StaticData.Progression
{
    [CreateAssetMenu(fileName = "GeometricProgression", menuName = "Static data/Progression", order = 0)]
    public class StaticDataGeometricProgression : BaseProgression
    {
        //  начальное значение
        public float InitialValue;
        // значение, на которое умножается предыдущее значение для получения следующего
        public float ProgressionCoefficient;


        public override float GetValue(int level) => 
            InitialValue * Mathf.Pow(ProgressionCoefficient, level - 1);
    }
}