using System;
using UnityEngine;

namespace CollisionFXUpdated
{
	internal class SparkLauncher : MonoBehaviour
	{
		private ParticleSystem _sparkSystem = null;

		void Start()
		{
			_sparkSystem = GetComponent<ParticleSystem>();

			if (_sparkSystem == null)
			{
				Debug.LogException(new NullReferenceException("CollisionFXUPdated: _sparkSystem could not find comonent"));
			}
			else
			{
				//_sparkSystem.
			}
		}
	}
}
