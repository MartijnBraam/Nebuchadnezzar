using System;
using MatrixSDK.Structures;
namespace MatrixSDK.Client
{
	/// <summary>
	/// A representation of a Matrix User.
	/// Contains basic profile information.
	/// </summary>
	public class MatrixUser
	{
		/// <summary>
		/// This constructor is intended for the API only.
		/// Create a new user from a profile & userid.
		/// </summary>
		/// <param name="Profile">Profile.</param>
		/// <param name="userid">Userid.</param>
		public MatrixUser(MatrixProfile Profile,string userid){
			profile = Profile;
			UserID = userid;
		}

		MatrixProfile profile;

		public string AvatarURL { get { return profile.avatar_url; } }
		public string DisplayName { get { return profile.displayname; } }
		public readonly string UserID;
	}
}

