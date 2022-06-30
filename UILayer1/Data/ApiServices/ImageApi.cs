using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class ImageApi
    {
        private IConfiguration Configuration { get; }
        // for testing ////////////////////////////firebase//////////////////////////////////
        private static string apiKey = "AIzaSyBvGbaacBA91vzQfmvUsF77eAJSYn6b4VE";
        private static string Bucket = "mobizone-55ea5.appspot.com";
        private static string AuthEmail = "subinbabu4127@gmail.com";
        private static string AuthPassword = "Subin@1999";
        //////////////////////////////////////////////////////
        public ImageApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IEnumerable<Images> Get()
        {
            try
            {
                RequestHandler<IEnumerable<Images>> _requestHandler = new RequestHandler<IEnumerable<Images>>(Configuration);
                _requestHandler.url = "api/imagesoperations";
                return _requestHandler.Get().result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id,string filename)
        {
try
            {
                string extractFilename = "";
                try
                {
                    extractFilename = Regex.Split(filename, @"%2F(.*?)\?alt")[1];
                }
                catch (Exception ex)
                {

                } 
                try
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                    var a = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword).Result;
                    var cancellationToken = new CancellationTokenSource();
                    var upload = new FirebaseStorage(Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true
                        })
                        .Child("assets")
                        .Child(extractFilename)
                        .DeleteAsync();
                    await upload;
                }
                catch (Exception ex)
                {

                }
                RequestHandler<string> requestHandler = new RequestHandler<string>(Configuration);
                requestHandler.url = "api/imagesoperations/";
                var result =requestHandler.Delete(id);
                if(result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
