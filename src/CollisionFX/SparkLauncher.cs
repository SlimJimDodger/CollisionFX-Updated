//-------------------------------------------------------------
// Collision FX-Updated
// Author:    SlimJimDodger
// Version:   5.0a
// Released:  2019-01-22
// KSP:       v1.6.1

// Thread:    http://forum.kerbalspaceprogram.com/
// Licence:   GNU v2, http://www.gnu.org/licenses/gpl-2.0.html
// Source:    https://github.com/SlimJimdDodger/CollisionFX-Updated
//-------------------------------------------------------------

using System;
using UnityEngine;

namespace CollisionFXUpdated
{
	public class SparkLauncher : MonoBehaviour
	{
		private ParticleSystem _sparkSystem = null;
		private GameObject _sparkObj = null;
		private GameObject _lightObj = null;
		private GameObject _meshObj = null;
		private Light _contactPtLight = null;
		private Rigidbody _rigidBody = null;

		ParticleSystem.EmissionModule _emission;
		ParticleSystem.LightsModule _particleLights;
		private ParticleSystem.SizeOverLifetimeModule _sizeoverlifetime;

		private float waitTime = 0.5f;
		private float timer = 0.0f;
		private bool _instantiated = false;
		
		public SparkLauncher sparkLauncher;

		#region Events

		void Start()
		{
			_lightObj = new GameObject("cfx_light");
			_meshObj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Effects/fx_exhaustSparks_flameout"));
			//_sparkSystem = _sparkObj.GetComponent<ParticleSystem>();

			_sparkObj = new GameObject("sparks4u");
			_sparkObj.transform.parent = this.transform.parent;
			_sparkObj.transform.Rotate(-this.transform.parent.transform.forward);

			_rigidBody = _sparkObj.AddComponent<Rigidbody>();
			_rigidBody.useGravity = true;
			_rigidBody.transform.parent = _sparkObj.transform;

			_sparkSystem = new ParticleSystem();
			_sparkSystem = _sparkObj.AddComponent<ParticleSystem>();

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
					main.duration = 5.0f;
					// replay at end of main.duration seconds
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

					 _emission = _sparkSystem.emission;
					// how many per second / how much flow 
					_emission.rateOverTime = 20f;
					_emission.enabled = false;

					var shape = _sparkSystem.shape;
					shape.enabled = true;

					//shape.shapeType = ParticleSystemShapeType.Sphere;
					//shape.radius = 0.1f;
					//shape.scale = Vector3.one;

					//_meshObj = new GameObject();
					//var meshRenderer = _sparkObj.GetComponent<MeshRenderer>();
					//shape.meshRenderer = meshRenderer;
					//meshRenderer.enabled = true;
					////meshRenderer.
					//var meshFilter = _sparkObj.GetComponent<MeshFilter>();
					//meshFilter.mesh = CreateSphere(null);
					//shape.mesh = meshFilter.mesh;

					shape.shapeType = ParticleSystemShapeType.Cone;
					shape.angle = 40f;
					shape.radius = 0.28f;
					shape.radiusThickness = 1;
					shape.arc = 180f;
					shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
					shape.arcSpread = 0;

					var inheritVelocity = _sparkSystem.inheritVelocity;
					inheritVelocity.enabled = true;
					inheritVelocity.mode = ParticleSystemInheritVelocityMode.Initial;
					inheritVelocity.curveMultiplier = 0.75f;

					var velOverLifetime = _sparkSystem.velocityOverLifetime;
					velOverLifetime.enabled = false;
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

					_sizeoverlifetime = _sparkSystem.sizeOverLifetime;
					_sizeoverlifetime.enabled = true;
					_sizeoverlifetime.size = new ParticleSystem.MinMaxCurve(1, animCurve);

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
					systemRender.velocityScale = 0.002f;
					systemRender.lengthScale = 1.3f;
					systemRender.material = _meshObj.GetComponent<Renderer>().material;// UnityEngine.Resources.Load("Effects/fx_exhaustSparks_flameout");

					//_lightObj.transform.parent = transform;
					var light = _lightObj.AddComponent<Light>();
					light.type = LightType.Point;
					light.range = 1;
					light.intensity = 3f;
					light.color = new Color32(255, 157, 82, 255);
					light.enabled = false;

					_particleLights = _sparkSystem.lights;
					_particleLights.light = light;
					_particleLights.ratio = 0.5f;
					_particleLights.rangeMultiplier = 5;
					_particleLights.intensityMultiplier = 5;
					_particleLights.useParticleColor = false;
					_particleLights.maxLights = 10000;

					_contactPtLight = _sparkObj.AddComponent<Light>();
					_contactPtLight.transform.parent = _sparkObj.transform;
					_contactPtLight.type = LightType.Point;
					_contactPtLight.range = 1;
					_contactPtLight.intensity = 3f;
					_contactPtLight.color = new Color32(255, 153, 0, 255);
					_contactPtLight.enabled = true;

					_instantiated = true;
					DoLights(false);
					DoSparks(false, 0);
					//_sparkSystem.Play();
				}
				catch (Exception ex)
				{
					Debug.LogException(new Exception(String.Format("CollisionFXUPdated: Error in SparkLauncher Setup -- > {0}", ex), ex));
				}
			}
		}

		void Update()
		{
			timer += Time.deltaTime;

			//DoLights(transform.forward.magnitude > 5f);

			// Check if we have reached beyond 2 seconds.
			// Subtracting two is more accurate over time than resetting to zero.
			if (timer > waitTime)
			{
				//_sparkSystem.Emit(UnityEngine.Random.Range(200, 1000));
				if (Time.frameCount % 10 == 0)
				{
					DoLights(false);
					//DoSparks(false);
					//var emission = _sparkSystem.emission;
					////if (_sparkSystem.isPlaying)
					//if (_sparkSystem.isEmitting)
					//{
					//	emission.enabled = false;
					//}
					//else
					//{
					//	emission.enabled = true;
					//}
					
				}
				// Remove the recorded 2 seconds.
				timer = timer - waitTime;
			}
			//_sparkObj.transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
			//_sparkObj.transform.Rotate(-transform.parent.transform.forward);
		}

		void OnDestroy()
		{
			if (_meshObj != null)
				Destroy(_meshObj);
		}
		#endregion

		private void DoLights(bool lightOn)
		{
			_contactPtLight.enabled = lightOn;
			
			//var light = _particleLights.light;
			_particleLights.light.enabled = lightOn;
		}

		private void DoSparks(bool sparksOn, float collisionSpeed)
		{
			var multiplier = 10f;
			//_emission.rateOverTime = 1000f;
			//_emission.enabled = sparksOn;
			var main = _sparkSystem.main;
			main.startLifetime = 5f;//collisionSpeed / multiplier;
			//main.startSize = collisionSpeed / multiplier;
			//_sizeoverlifetime.sizeMultiplier = collisionSpeed / multiplier;
			_sparkSystem.Emit((int)collisionSpeed * 10);
		}

		public void DoCollision(Vector3 contactPoint, float collisionSpeed, bool doSpark)
		{
			if (_instantiated)
			{
				_sparkSystem.transform.position = contactPoint;
				_sparkSystem.transform.Rotate(-transform.forward);
				_sparkSystem.transform.Rotate(new Vector3(0, 1, 0), 30f);
				_contactPtLight.transform.position = contactPoint;

				DoLights(doSpark);
				DoSparks(doSpark, collisionSpeed);
				//_sparkSystem.emit(5000);
			}
		}

		#region 
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

		static Mesh CreateSphere(Mesh mesh)
		{
			//MeshFilter filter = gameObject.AddComponent<MeshFilter>();
			//Mesh mesh = filter.mesh;
			if (mesh == null)
				mesh = new Mesh();
			else
				mesh.Clear();

			float radius = 1f;
			// Longitude |||
			int nbLong = 24;
			// Latitude ---
			int nbLat = 16;

			#region Vertices
			Vector3[] vertices = new Vector3[(nbLong + 1) * nbLat + 2];
			float _pi = Mathf.PI;
			float _2pi = _pi * 2f;

			vertices[0] = Vector3.up * radius;
			for (int lat = 0; lat < nbLat; lat++)
			{
				float a1 = _pi * (float)(lat + 1) / (nbLat + 1);
				float sin1 = Mathf.Sin(a1);
				float cos1 = Mathf.Cos(a1);

				for (int lon = 0; lon <= nbLong; lon++)
				{
					float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
					float sin2 = Mathf.Sin(a2);
					float cos2 = Mathf.Cos(a2);

					vertices[lon + lat * (nbLong + 1) + 1] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
				}
			}
			vertices[vertices.Length - 1] = Vector3.up * -radius;
			#endregion

			#region Normales		
			Vector3[] normales = new Vector3[vertices.Length];
			for (int n = 0; n < vertices.Length; n++)
				normales[n] = vertices[n].normalized;
			#endregion

			#region UVs
			Vector2[] uvs = new Vector2[vertices.Length];
			uvs[0] = Vector2.up;
			uvs[uvs.Length - 1] = Vector2.zero;
			for (int lat = 0; lat < nbLat; lat++)
				for (int lon = 0; lon <= nbLong; lon++)
					uvs[lon + lat * (nbLong + 1) + 1] = new Vector2((float)lon / nbLong, 1f - (float)(lat + 1) / (nbLat + 1));
			#endregion

			#region Triangles
			int nbFaces = vertices.Length;
			int nbTriangles = nbFaces * 2;
			int nbIndexes = nbTriangles * 3;
			int[] triangles = new int[nbIndexes];

			//Top Cap
			int i = 0;
			for (int lon = 0; lon < nbLong; lon++)
			{
				triangles[i++] = lon + 2;
				triangles[i++] = lon + 1;
				triangles[i++] = 0;
			}

			//Middle
			for (int lat = 0; lat < nbLat - 1; lat++)
			{
				for (int lon = 0; lon < nbLong; lon++)
				{
					int current = lon + lat * (nbLong + 1) + 1;
					int next = current + nbLong + 1;

					triangles[i++] = current;
					triangles[i++] = current + 1;
					triangles[i++] = next + 1;

					triangles[i++] = current;
					triangles[i++] = next + 1;
					triangles[i++] = next;
				}
			}

			//Bottom Cap
			for (int lon = 0; lon < nbLong; lon++)
			{
				triangles[i++] = vertices.Length - 1;
				triangles[i++] = vertices.Length - (lon + 2) - 1;
				triangles[i++] = vertices.Length - (lon + 1) - 1;
			}
			#endregion

			mesh.vertices = vertices;
			mesh.normals = normales;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			mesh.RecalculateBounds();
			return mesh;
		}
		#endregion
	}
}