using System;
using UnityEngine;

namespace CollisionFXUpdated
{
	public class SparkLauncher : MonoBehaviour
	{
		private ParticleSystem _sparkSystem = null;
		private GameObject _sparkObj = null;

		public SparkLauncher sparkLauncher;

		private float waitTime = 0.5f;
		private float timer = 0.0f;

		void Start()
		{
			_sparkObj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Effects/fx_exhaustSparks_flameout"));
			_sparkObj.transform.parent = this.transform.parent;
			_sparkObj.transform.position = -this.transform.parent.transform.forward;
			//_sparkObj = new GameObject();
			_sparkSystem = _sparkObj.GetComponent<ParticleSystem>();

			if (_sparkSystem == null)
			{
				Debug.LogException(new NullReferenceException("CollisionFXUPdated: _sparkSystem could not find ParticleSystem component"));
			}
			else
			{
				try
				{
					var main = _sparkSystem.main;

					// length of time particles are emitted, control this later
					main.duration = 0.0f;
					// replay every main.duration seconds?
					main.loop = false;
					// time of particle, die when reach 0
					main.startLifetime = 2f;
					// starting size of particle
					main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.25f);
					main.startSpeed = 4.5f;

					main.startColor = (Color)new Color32(255, 192, 98, 255);

					main.gravityModifier = 1;
					// particles are released into the world
					main.simulationSpace = ParticleSystemSimulationSpace.World;
					// don't start on start
					main.playOnAwake = false;
					main.maxParticles = 10000;
					main.scalingMode = ParticleSystemScalingMode.Local;


					var emission = _sparkSystem.emission;
					// how many per second 
					emission.rateOverTime = 20f;

					var shape = _sparkSystem.shape;
					shape.enabled = false;
					shape.shapeType = ParticleSystemShapeType.Sphere;
					shape.radius = 0.1f;
					shape.scale = Vector3.one;
					//shape.shapeType = ParticleSystemShapeType.Cone;
					//shape.angle = 40f;
					//shape.radius = 0.28f;
					//shape.radiusThickness = 1;
					//shape.arc = 180f;
					//shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
					//shape.arcSpread = 0;

					var inheritVelocity = _sparkSystem.inheritVelocity;
					inheritVelocity.enabled = true;
					inheritVelocity.mode = ParticleSystemInheritVelocityMode.Initial;
					inheritVelocity.curveMultiplier = 0.75f;


					var velOverLifetime = _sparkSystem.velocityOverLifetime;
					velOverLifetime.enabled = true;
					velOverLifetime.space = ParticleSystemSimulationSpace.World;
					velOverLifetime.x = 5;

					var colorOverLifetime = _sparkSystem.colorOverLifetime;
					colorOverLifetime.enabled = true;
					//colorOverLifetime.color = new ParticleSystem.MinMaxGradient((Color)new Color32(255, 255, 0, 255), (Color)new Color32(165, 153, 0, 0));

					float alpha = 1.0f;
					var grad = new Gradient();
					grad.SetKeys(
						 new GradientColorKey[] { new GradientColorKey(new Color32(255, 244, 232, 255), 0.0f), new GradientColorKey(new Color32(255, 23, 23, 0), 0.3f), new GradientColorKey(new Color32(255, 23, 23, 0), 1.0f) },
						 new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(alpha, 0.0f) }
					);
					colorOverLifetime.color = grad;

					var animCurve = new AnimationCurve();
					animCurve.AddKey(0.0f, 0.5f);
					animCurve.AddKey(0.1f, 0.6f);
					animCurve.AddKey(0.2f, 0.7f);
					animCurve.AddKey(0.3f, 0.5f);
					animCurve.AddKey(0.5f, 0.4f);
					animCurve.AddKey(0.7f, 0.3f);
					animCurve.AddKey(0.9f, 0.2f);
					animCurve.AddKey(1.0f, 0.1f);

					var sizeoverlifetime = _sparkSystem.sizeOverLifetime;
					sizeoverlifetime.enabled = true;
					sizeoverlifetime.size = new ParticleSystem.MinMaxCurve(1, animCurve);

					var collisions = _sparkSystem.collision;
					collisions.enabled = true;
					collisions.mode = ParticleSystemCollisionMode.Collision3D;
					collisions.type = ParticleSystemCollisionType.World;
					collisions.dampen = 0.25f;
					collisions.bounce = 0.2f;
					collisions.minKillSpeed = 0;
					collisions.maxKillSpeed = 10000;
					collisions.radiusScale = 1.0f;
					collisions.quality = ParticleSystemCollisionQuality.High;
					collisions.collidesWith = 1;
					collisions.maxCollisionShapes = 256;
					collisions.enableDynamicColliders = true;

					var systemRender = _sparkSystem.GetComponent<ParticleSystemRenderer>();
					systemRender.renderMode = ParticleSystemRenderMode.Stretch;
					systemRender.velocityScale = 0.02f;
					systemRender.lengthScale = 1.0f;

					var light = new Light();
					light.type = LightType.Point;
					light.range = 4;
					light.color = new Color32(255, 157, 82, 255);

					var lights = _sparkSystem.lights;
					lights.enabled = true;
					lights.light = light;
					lights.ratio = 0.05f;
					lights.rangeMultiplier = 5;
					lights.intensityMultiplier = 2;
					lights.useParticleColor = false;
					lights.maxLights = 10;

					var srclight = _sparkObj.AddComponent<Light>();
					srclight.type = LightType.Point;
					srclight.range = 7;
					srclight.intensity = 3;
					srclight.color = new Color32(255, 153, 0, 255);

				}
				catch (Exception ex)
				{
					Debug.LogException(new Exception("CollisionFXUPdated: Error in SparkLauncher Setup", ex));
				}

				//_sparkSystem.Play();
			}
		}

		void Update()
		{
			timer += Time.deltaTime;

			// Check if we have reached beyond 2 seconds.
			// Subtracting two is more accurate over time than resetting to zero.
			if (timer > waitTime)
			{
				//if (Time.frameCount % 2 == 0)
				//{

				//if (_sparkSystem.isPlaying)
				//	_sparkSystem.Pause();
				//else
				//	_sparkSystem.Play();
				_sparkObj.transform.Rotate(-transform.parent.transform.forward);
				_sparkSystem.Emit(UnityEngine.Random.Range(200, 1000));
				//}
				// Remove the recorded 2 seconds.
				timer = timer - waitTime;
			}
			//_sparkObj.transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
		}

		static ParticleSystem.MinMaxCurve GenerateSizeGrowCurve(float sizeGrow, float minTime, float maxTime)
		{
			const float step = 0.2f;

			if (sizeGrow > 0)
			{
				if (Mathf.Approximately(minTime, maxTime))
				{
					float finalSize = Mathf.Pow(1.0f + sizeGrow, minTime);
					var animCurve = new AnimationCurve();
					for (float time = 0; time <= 1.0f; time += step)
					{
						animCurve.AddKey(time, Mathf.Pow(1 + sizeGrow, time * maxTime) / finalSize);
					}

					// Remove keys used to create the shape
					while (animCurve.keys.Length > 2)
						animCurve.RemoveKey(1);

					return new ParticleSystem.MinMaxCurve(finalSize, animCurve);
				}
				else
				{
					float finalSizeMax = Mathf.Pow(1.0f + sizeGrow, maxTime);

					var animCurveMin = new AnimationCurve();
					var animCurveMax = new AnimationCurve();
					for (float time = 0; time <= 1.0f; time += step)
					{
						animCurveMin.AddKey(time, Mathf.Pow(1 + sizeGrow, time * minTime) / finalSizeMax);
						animCurveMax.AddKey(time, Mathf.Pow(1 + sizeGrow, time * maxTime) / finalSizeMax);
					}

					while (animCurveMin.keys.Length > 2)
					{
						animCurveMin.RemoveKey(1);
						animCurveMax.RemoveKey(1);
					}

					return new ParticleSystem.MinMaxCurve(finalSizeMax, animCurveMin, animCurveMax);
				}
			}
			else
			{
				if (Mathf.Approximately(minTime, maxTime))
				{
					var animCurve = new AnimationCurve();
					for (float time = 0; time <= 1.0f; time += step)
					{
						animCurve.AddKey(time, Mathf.Pow(-sizeGrow, time));
					}

					// Remove keys used to create the shape
					while (animCurve.keys.Length > 2)
						animCurve.RemoveKey(1);

					return new ParticleSystem.MinMaxCurve(1, animCurve);
				}
				else
				{
					var animCurveMin = new AnimationCurve();
					var animCurveMax = new AnimationCurve();
					float minTimeOverMaxTime = minTime / maxTime;
					for (float time = 0; time <= 1.0f; time += step)
					{
						animCurveMin.AddKey(time, Mathf.Pow(-sizeGrow, time * minTimeOverMaxTime));
						animCurveMax.AddKey(time, Mathf.Pow(-sizeGrow, time));
					}

					while (animCurveMin.keys.Length > 2)
					{
						animCurveMin.RemoveKey(1);
						animCurveMax.RemoveKey(1);
					}

					return new ParticleSystem.MinMaxCurve(1, animCurveMin, animCurveMax);
				}
			}
		}
	}
}
