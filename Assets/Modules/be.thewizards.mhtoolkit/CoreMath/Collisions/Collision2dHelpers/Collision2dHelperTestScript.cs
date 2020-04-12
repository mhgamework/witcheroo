using Assets.CustomGridPathfindingTest;
using Assets.Utils;
using MHGameWork.TheWizards;
using UnityEngine;

namespace Assets.PathfindingAstarProject
{
    public class Collision2dHelperTestScript : MonoBehaviour
    {
        public Transform Center;
        public Transform End;
        public Transform ObstacleStart;
        public Transform ObstacleEnd;
        public float Width;

        public bool DrawAABB = true;

        public void Update()
        {
            var center = Center.transform.position.TakeXZ();
            var dir = (End.position - Center.position).normalized.TakeXZ();
            var dist = (End.position - Center.position).magnitude;
            var obstacle = new Rect(ObstacleStart.position.TakeXZ(), (ObstacleEnd.position - ObstacleStart.position).TakeXZ());

            var rect = new OrientedRect(center, dir, dist, Width);


            var hit = Collision2DHelper.Intersects(obstacle, rect);




            DrawHelpers.DrawRect(rect, hit ? Color.red : Color.green);

            DrawHelpers.DrawRect(obstacle, Color.blue);
            DrawHelpers.DrawRect(rect.GetAABB(), Color.black);
        }

    }
}