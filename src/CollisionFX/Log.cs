using System;
using System.IO;

namespace CollisionFXUpdated
{
	class Log
	{
		#region log
		public static void WriteLog(string strMessage)
		{
			if (!String.IsNullOrEmpty(strMessage))
			{
				FileStream objFilestream = new FileStream(CollisionFX.LogFile, FileMode.Append, FileAccess.Write);
				StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
				objStreamWriter.AutoFlush = true;
				objStreamWriter.WriteLine(strMessage);
				objStreamWriter.Close();
				objFilestream.Close();
			}
		}

		public static void InitLog()
		{
			FileStream objFilestream = new FileStream(CollisionFX.LogFile, FileMode.Create, FileAccess.Write);
			StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
			objStreamWriter.AutoFlush = true;
			objStreamWriter.WriteLine(String.Format("CollisionFxUpdated\n{0}\n",DateTime.Now));
			objStreamWriter.WriteLine(Path.GetFullPath(Path.Combine(CollisionFX.AssemblyPath, ".." + Path.DirectorySeparatorChar + ".. " + Path.DirectorySeparatorChar)));
			objStreamWriter.WriteLine("\n\n");
			objStreamWriter.Close();
			objFilestream.Close();
		}
		#endregion
	}
}
