﻿using System;
using System.Threading.Tasks;
using Google;
using Pibrary.Config;
using UniRx;
using Unity;
using UnityEngine;

namespace Pibrary.Auth
{
    public class TestAuthHandler : IAuthHandler
    {
        private Subject<LoadingState> stateSubject = new Subject<LoadingState>();
        public IObservable<LoadingState> OnStateChanged
        {
            get { return stateSubject; }
        }

        private bool initialized;
        
        public async void CallGoogleSignIn()
        {
            if (!initialized)
            {
                string clientID = ConfigProvider.OAuthConfig.cliendID;
                Debug.Log(clientID);
                GoogleSignIn.Configuration = new GoogleSignInConfiguration {
                    RequestIdToken = true,
                    // Copy this value from the google-service.json file.
                    // oauth_client with type == 3
                    WebClientId = clientID
                };
                initialized = true;
            }
            
            stateSubject.OnNext(LoadingState.Loading);
            Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

            signIn.ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.Log("GoogleSignIn was canceled.");
                } else if (task.IsFaulted) {
                    Debug.Log("GoogleSignIn was error.");
                } else {
                    // Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                }
            });
        }

        public void CallEmailSignIn(string email, string password)
        {
            stateSubject.OnNext(LoadingState.Loading);
            
        }
    }
}