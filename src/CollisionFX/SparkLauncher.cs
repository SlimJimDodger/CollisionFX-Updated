//-------------------------------------------------------------
// Collision FX-Updated
// Author:    SlimJimDodger
// Version:   0.8.2
// Released:  2019-01-22
// KSP:       v1.6.1

// Thread:    https://forum.kerbalspaceprogram.com/index.php?/topic/181664-140-16x-collisionfx-updated-081/
// Licence:   GNU v2, http://www.gnu.org/licenses/gpl-2.0.html
// Source:    https://github.com/SlimJimdDodger/CollisionFX-Updated
//-------------------------------------------------------------

using System;
using System.IO;
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
		private ParticleSystem.TrailModule _trails;

		private float waitTime = 0.5f;
		private float timer = 0.0f;
		private bool _instantiated = false;

		public SparkLauncher Launcher { get => this; }
		private float _startSize = 0.25f;
		private ParticleSystem.EmitParams _emitParams = new ParticleSystem.EmitParams();

		#region Events

		// TODO: unity minmax curve script manual shows cool things

		void Start()
		{
			_lightObj = new GameObject("cfx_light");
			_meshObj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load("Effects/fx_exhaustFlare_yellow"));
			//_meshObj = (GameObject)GameObject.Instantiate(UnityEngine.Resources.Load(Path.Combine(typeof(CollisionFX).Assembly.Location, "spark.png")));
			
			//_sparkSystem = _sparkObj.GetComponent<ParticleSystem>();

			_sparkObj = new GameObject("sparks4u");			
			_sparkObj.transform.parent = this.transform.parent;
			_sparkObj.transform.Rotate(-this.transform.parent.transform.forward);

			// _rigidBody = _sparkObj.AddComponent<Rigidbody>();
			var transformParent = this.transform.parent;
			Part part = transform.parent.GetComponentInParent<Part>();
			
			//_rigidBody = part.GetComponent<Rigidbody>();//.GetComponentInParent<Rigidbody>();
			//if(_rigidBody = null)
			//	part.RigidBodyPart.ri
			//_rigidBody.useGravity = true;
			//_rigidBody.transform.parent = _sparkObj.transform.parent;

			_sparkSystem = new ParticleSystem();
			_sparkSystem = _sparkObj.AddComponent<ParticleSystem>();
			//_sparkSystem.transform.parent = _rigidBody.transform;

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
					main.duration = 1.0f;
					// replay at end of main.duration seconds
					main.loop = false;
					// time of particle, die when reach 0
					main.startLifetime = 2f;
					// starting size of particle
					main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.25f);
					main.startSpeed = 1f;


					main.startColor = (Color)new Color32(255, 192, 98, 255);

					main.gravityModifier = 2;
					// particles are released into the world
					main.simulationSpace = ParticleSystemSimulationSpace.World;
					// don't start on start
					main.playOnAwake = false;
					main.maxParticles = 10000;
					main.scalingMode = ParticleSystemScalingMode.Local;

					 _emission = _sparkSystem.emission;
					// how many per second / how much flow 
					_emission.rateOverTime = 500f;
					_emission.enabled = false;

					#region shape
					var shape = _sparkSystem.shape;
					shape.enabled = true;

					shape.shapeType = ParticleSystemShapeType.Sphere;
					shape.radius = 0.1f;
					shape.scale = Vector3.one;

					//_meshObj = new GameObject();
					//var meshRenderer = _sparkObj.GetComponent<MeshRenderer>();
					//shape.meshRenderer = meshRenderer;
					//meshRenderer.enabled = true;
					////meshRenderer.
					//var meshFilter = _sparkObj.GetComponent<MeshFilter>();
					//meshFilter.mesh = CreateSphere(null);
					//shape.mesh = meshFilter.mesh;

					//shape.shapeType = ParticleSystemShapeType.Cone;
					//shape.angle = 20f;
					//shape.radius = 0.01f;
					//shape.radiusThickness = 1;
					//shape.arc = 180f;
					//shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
					//shape.arcSpread = 0;
					#endregion

					var inheritVelocity = _sparkSystem.inheritVelocity;
					inheritVelocity.enabled = false;
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
						 new GradientColorKey[] { new GradientColorKey(new Color32(255, 244, 232, 255), 0.0f), new GradientColorKey(new Color32(255, 100, 100, 0), 0.3f), new GradientColorKey(new Color32(255, 23, 23, 0), 1.0f) },
						 new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(alpha, 0.0f) }
					);
					colorOverLifetime.color = grad;

					var animCurve = new AnimationCurve();
					//animCurve.AddKey(0.0f, 0.5f);
					//animCurve.AddKey(0.1f, 0.6f);
					//animCurve.AddKey(0.2f, 0.7f);
					//animCurve.AddKey(0.3f, 0.5f);
					//animCurve.AddKey(0.5f, 0.4f);
					//animCurve.AddKey(0.7f, 0.3f);
					//animCurve.AddKey(0.9f, 0.2f);
					//animCurve.AddKey(1.0f, 0.1f);

					animCurve.AddKey(0.0f, 0.4f);
					animCurve.AddKey(1.0f, 0.01f);

					_sizeoverlifetime = _sparkSystem.sizeOverLifetime;
					_sizeoverlifetime.enabled = true;
					//_sizeoverlifetime.size = GenerateSizeGrowCurve(0.01f, 0f, 5f);
					_sizeoverlifetime.size = new ParticleSystem.MinMaxCurve(1, animCurve);

					#region collisions
					var collisions = _sparkSystem.collision;
					collisions.enabled = true;
					collisions.mode = ParticleSystemCollisionMode.Collision3D;
					collisions.type = ParticleSystemCollisionType.World;
					collisions.dampen = 0.1f;
					collisions.bounce = 0.7f;
					collisions.minKillSpeed = 1;
					collisions.maxKillSpeed = 1000;
					collisions.radiusScale = .25f;
					collisions.quality = ParticleSystemCollisionQuality.High;
					collisions.collidesWith = 1;
					collisions.maxCollisionShapes = 256;
					collisions.enableDynamicColliders = true;
					#endregion

					var systemRender = _sparkSystem.GetComponent<ParticleSystemRenderer>();
					systemRender.renderMode = ParticleSystemRenderMode.Stretch;
					systemRender.velocityScale = .1f;
					systemRender.lengthScale = 1f;

					//string path = Path.Combine(typeof(CollisionFX).Assembly.Location, "spark.png");
					//byte[] fileData = File.ReadAllBytes(path);
					//Texture2D tex = new Texture2D(64, 64);
					//tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
					
					systemRender.material = _meshObj.GetComponent<Renderer>().material;// UnityEngine.Resources.Load("Effects/fx_exhaustSparks_flameout");
					//systemRender.material.mainTexture = tex;
					systemRender.trailMaterial = _meshObj.GetComponent<Renderer>().material;
					//systemRender.minParticleSize;
					//systemRender.maxParticleSize;
					//systemRender.normalDirection = 0;

					#region lights
					//_lightObj.transform.parent = transform;
					var light = _lightObj.AddComponent<Light>();
					light.type = LightType.Point;
					light.range = .1f;
					light.intensity = 2f;
					light.color = new Color32(255, 157, 82, 255);
					light.enabled = true;
					light.renderMode = LightRenderMode.ForcePixel;

					_particleLights = _sparkSystem.lights;
					_particleLights.light = light;
					_particleLights.ratio = 1f;
					_particleLights.rangeMultiplier = 1;
					_particleLights.intensityMultiplier = 1;
					_particleLights.useParticleColor = false;
					_particleLights.maxLights = 10000;
					_particleLights.enabled = true;

					_contactPtLight = _sparkObj.AddComponent<Light>();
					_contactPtLight.transform.parent = _sparkObj.transform;
					_contactPtLight.type = LightType.Point;
					_contactPtLight.range = 0.1f;
					_contactPtLight.intensity = 2f;
					_contactPtLight.color = new Color32(255, 153, 0, 255);
					_contactPtLight.enabled = true;
					#endregion

					#region trails
					_trails = _sparkSystem.trails;
					_trails.enabled = false;
					_trails.ratio = 0.1f;
					_trails.lifetime = .25f;
					_trails.textureMode = ParticleSystemTrailTextureMode.Stretch;
					//trails.dieWithParticles = true;
					_trails.sizeAffectsWidth = true;
					_trails.minVertexDistance = 0.2f;
					_trails.inheritParticleColor = true;
					_trails.worldSpace = true;
					_trails.sizeAffectsLifetime = false;
					_trails.widthOverTrail = 0.2f;
					//var grad2 = new Gradient();
					//grad2.SetKeys(
					//	 new GradientColorKey[] { new GradientColorKey(new Color32(255, 244, 232, 255), 0.0f), new GradientColorKey(new Color32(255, 23, 23, 0), 0.3f), new GradientColorKey(new Color32(255, 23, 23, 0), 1.0f) },
					//	 new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(alpha, 0.0f) }
					//);
					//colorOverLifetime.color = grad2;
					//trails.colorOverLifetime = grad;
					#endregion

					#region gravity
					var _externalforce = _sparkSystem.externalForces;
					_externalforce.enabled = true;

					var forceOverLifetime = _sparkSystem.forceOverLifetime;
					forceOverLifetime.enabled = true;
					forceOverLifetime.space = ParticleSystemSimulationSpace.World;
					forceOverLifetime.x = Physics.gravity.x;
					forceOverLifetime.y = Physics.gravity.y;
					forceOverLifetime.z = Physics.gravity.z;
					#endregion

					_instantiated = true;

					DoLights(false);
				}
				catch (Exception ex)
				{
					Debug.LogException(new Exception(String.Format("CollisionFXUPdated: Error in SparkLauncher Setup -- > {0}", ex), ex));
				}
			}
		}

		void Update()
		{
			// Check if we have reached beyond 2 seconds.
			// Subtracting two is more accurate over time than resetting to zero.
			if (timer > waitTime)
			{
				if (Time.frameCount % 2 == 0)
				{
					if (!_sparkSystem.isEmitting || _rigidBody.velocity.magnitude < 1)
					{
						DoLights(false);
					}
				}
				// Remove the recorded 2 seconds.
				timer = timer - waitTime;
			}
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
			_particleLights.light.enabled = lightOn;
			_particleLights.enabled = lightOn;
		}

		private void DoSparks(bool sparksOn, float collisionSpeed)
		{
			var lifetime = Mathf.Clamp(collisionSpeed / 10, .3f, 2f);
			var main = _sparkSystem.main;

			_contactPtLight.range = Mathf.Clamp(lifetime, 0.01f, 0.5f);
			_contactPtLight.intensity = Mathf.Clamp(collisionSpeed, .5f, 3f);
			_particleLights.light.intensity = _contactPtLight.intensity;

			main.startLifetime = lifetime;
			main.duration = lifetime;
			
			var animCurve = new AnimationCurve();
			animCurve.AddKey(0.0f, _startSize);
			animCurve.AddKey(lifetime, 0.0f);
			_sizeoverlifetime.size = new ParticleSystem.MinMaxCurve(1, animCurve); ;
			//main.startSize = startSize;

			_trails.enabled = false;
			_trails.lifetime = lifetime /4 ;
			_trails.ratio = .3f;
			//_trails.dieWithParticles = true;

			_emitParams.velocity = 1.25f * collisionSpeed * _rigidBody.transform.forward;
			_emitParams.startLifetime = lifetime;

			var emitcount = 0;
			//if (collisionSpeed < 5f)
			//{
			//	emitcount = 1;  // this is too small
			//}
			//else
			//{
			//	emitcount = (int)(collisionSpeed / 2);
			//}

			emitcount = (int)Mathf.Clamp(collisionSpeed, 5f, 80f);

			if (sparksOn && !_sparkSystem.isEmitting)
			{
				//Log.WriteLog(String.Format("Sparking: {0} sparks", emitcount));
				//DumpObject(_emitParams, collisionSpeed);
				_sparkSystem.Emit(_emitParams, emitcount);
			}
		}

		public void DoCollision(Rigidbody rigidbody, Vector3 contactPoint, float collisionSpeed, bool doSpark)
		{
			if (_instantiated)
			{
				if (_rigidBody == null)
				{
					_rigidBody = rigidbody;
					_sparkSystem.transform.parent = _rigidBody.transform;
					_rigidBody.useGravity = true;
				}
				_contactPtLight.transform.position = contactPoint;
				_sparkSystem.transform.position = contactPoint;
				_sparkSystem.transform.Rotate(_rigidBody.transform.forward);
				_sparkSystem.transform.LookAt(_rigidBody.transform);
				//_sparkSystem.transform.Rotate(30f, 0f, 0f);

				DoLights(doSpark);
				DoSparks(doSpark, collisionSpeed);
			}
		}

		public void ExitCollision()
		{
			DoLights(false);
		}

		private void DumpObject(ParticleSystem.EmitParams emitParams, float collisionSpeed)
		{
			var main = _sparkSystem.main;

			Log.WriteLog("\n*>*>*>*>*>*>*>*>");
			Log.WriteLog(String.Format("Duration: {0}", main.duration));
			Log.WriteLog(String.Format("Loop: {0}", main.loop));
			Log.WriteLog(String.Format("StartLifetime: {0}", MinMaxToString(main.startLifetime)));
			Log.WriteLog(String.Format("StartSize: {0}", MinMaxToString(main.startSize)));
			Log.WriteLog(String.Format("Ship Velocity: {0}", _sparkSystem.transform.forward));
			Log.WriteLog(String.Format("Velocity: {0}", MinMaxToString(main.startSpeed)));
			Log.WriteLog(String.Format("Velocity (P): {0}", emitParams.velocity));
			Log.WriteLog(String.Format("RigidBody Velocity: {0}", _rigidBody.velocity.ToString()));
			Log.WriteLog(String.Format("CollisionSpeed: {0}", collisionSpeed));
			Log.WriteLog("*<*<*<*<*<*<*<\n");
		}

		private string MinMaxToString(ParticleSystem.MinMaxCurve curve)
		{
			return string.Format("min {0} max {1}", curve.constantMin, curve.constantMax);
		}

		#region Static Helpers
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