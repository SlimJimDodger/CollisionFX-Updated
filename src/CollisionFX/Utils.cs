using System;
using UnityEngine;
using System.Globalization;

namespace CollisionFX
{
    public static class Utils
    {
        public static bool IsPQS(Collider collider)
        {
            if (collider == null) return false;
            return IsPQS(collider.name);
        }
        public static bool IsPQS(string colliderName)
        {
            // Test for PQS: Name in the form "Ab0123456789".
            Int64 n;
            // TODO: Find a better way of doing this.
            return (colliderName.Length == 12 && Int64.TryParse(colliderName.Substring(2, 10), out n)) ||
                (colliderName.Length == 11 && Int64.TryParse(colliderName.Substring(2, 9), out n)); // Can now be 9 digits long? 
        }

        public static bool IsRagdoll(GameObject g)
        {
            /*
            be_neck01
            be_spE01
            bn_l_arm01 1
            bn_l_elbow_a01
            bn_l_hip01
            bn_l_knee_b01
            bn_r_elbow_a01
            bn_r_hip01
            bn_r_knee_b01
            bn_spA01
            */
            // TODO: Find a better way of doing this.
            return g.name.StartsWith("bn_") || g.name.StartsWith("be_");
        }

        /// <summary>
        /// Contact points are from the previous frame. Add the velocity to get the correct position.
        /// </summary>
        /// <param name="contactPoint">The collision's contactPoint to be adjusted.</param>
        /// <param name="part">The part involved in the collision</param>
        /// <returns>The contact point, corrected for the current frame.</returns>
        public static Vector3 PointToCurrentFrame(Vector3 contactPoint, Part part)
        {
            Part p = part;
            while (part.Rigidbody == null && part.parent != null)
                p = p.parent;
            if (p.Rigidbody == null)
            {
                Debug.LogError("[CollisionFX] Part " + part.name + " does not contain a Rigidbody or have a parent part with a Rigidbody.");
                return contactPoint;
            }

            return contactPoint + (p.Rigidbody.velocity * Time.deltaTime);
        }

        public static string GetCurrentBiomeName(Vessel vessel)
        {
            CBAttributeMapSO biomeMap = FlightGlobals.currentMainBody.BiomeMap;
            CBAttributeMapSO.MapAttribute mapAttribute = biomeMap.GetAtt(vessel.latitude * Mathf.Deg2Rad, vessel.longitude * Mathf.Deg2Rad);
            return mapAttribute.name;
        }
    }
}
