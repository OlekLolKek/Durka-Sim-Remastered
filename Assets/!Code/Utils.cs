using UnityEngine;


namespace DurkaSimRemastered
{
    public static class Utils
    {
        public static Vector3 Change(this Vector3 original, object x = null, object y = null, object z = null)
        {
            return new Vector3(
                x == null ? original.x : (float) x, 
                y == null ? original.y : (float) y,
                z == null ? original.z : (float) z
                );
        }
        
        public static Vector2 Change(this Vector2 original, object x = null, object y = null)
        {
            return new Vector2(
                x == null ? original.x : (float) x, 
                y == null ? original.y : (float) y
            );
        }
    }
}