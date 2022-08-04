using DomainLayer;
using DomainLayer.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UILayer.Data.ApiServices;

namespace UILayer.EcomerceLibrary
{
    public class CartLib
    {
        List<MyCart> _carts = null;
        List<CartDetails> _cartDetails = null;
        IProductOpApi _opApi = null;
        IUserApi _userApi = null;
        IEnumerable<MyCart> productCartListFromDb = null;
        private readonly IDistributedCache _distributedCache;
        public CartLib(IProductOpApi opApi, IUserApi userApi, IDistributedCache distributedCache)
        {
            _opApi = opApi;
            _userApi = userApi;
            _distributedCache = distributedCache;
        }
        public List<CartDetails> GetCartDetailsList(UserRegistration user)
        {
            _carts = _userApi.GetCart().Result.ToList();
            _cartDetails = new List<CartDetails>();

            try
            {
                if (IsExist(user))
                {
                    var data = _carts.ToList().Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();

                    var count = 0;
                    foreach (var item in data.cartDetails)
                    {
                        _cartDetails.Add(item);

                    }
                }
                foreach (var data in _cartDetails)
                {
                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(data.productId)).FirstOrDefault();
                    data.product = product;
                }
            }
            catch (Exception ex)

            {

            }
            return _cartDetails;
        }
        public List<CartDetails> GetCartDetailsList(string sessionId)
        {
            try
            {
                _carts = CartFromSession();

                _cartDetails = new List<CartDetails>();
                if (_carts.ToList().Where(c => c.sessionId.Equals(sessionId)) != null)
                {
                    var data = _carts.ToList().Where(c => c.sessionId.Equals(sessionId));

                    var count = 0;
                    foreach (var item in data)
                    {
                        if (item.sessionId.Equals(sessionId))
                        {
                            _cartDetails.Add(item.cartDetails.FirstOrDefault());
                        }
                    }
                }
                foreach (var data in _cartDetails)
                {
                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(data.productId)).FirstOrDefault();
                    data.product = product;
                }
            }
            catch (Exception ex)

            {

            }
            return _cartDetails;
        }
        public bool IsExist(UserRegistration user)
        {
            productCartListFromDb = _userApi.GetCart().Result;
            if (productCartListFromDb.Any(c => c.usersId.Equals(user.UserId)))
            {
                return true;
            }
            return false;
        }
        public bool AddtoCart(int id, UserRegistration user, string sessionId)

        {

            try
            {
                CartDetails cartDetails = new CartDetails();
                MyCart cart = new MyCart();
                cartDetails.productId = id;
                cartDetails.quantity = 1;
                var productData = _opApi.GetAll().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
                cartDetails.price = 1 * productData.price;
                IEnumerable<MyCart> productCartListFromDb = _userApi.GetCart().Result;
                if (productCartListFromDb.Any(c => c.usersId.Equals(user.UserId)))
                {
                    var productCartByUserId = productCartListFromDb.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                    var cartDetailslList = productCartByUserId.cartDetails;
                    if (cartDetailslList.Count == 0)
                    {
                        cartDetailslList.Add(cartDetails);
                    }
                    else
                    {
                        if (cartDetailslList.ToList().Any(c => c.productId.Equals(id)))
                        {
                            foreach (var cartDetailsData in cartDetailslList.ToList())
                            {
                                if (cartDetailsData.productId.Equals(id))
                                {
                                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
                                    cartDetailsData.quantity = cartDetailsData.quantity + 1;
                                    cartDetailsData.price = cartDetailsData.quantity * product.price;
                                }
                            }
                        }
                        else
                        {
                            cartDetailslList.Add(cartDetails);
                        }

                    }

                    productCartByUserId.cartDetails = cartDetailslList;
                    _userApi.EditCart(productCartByUserId);
                }
                else
                {
                    //_cartDetails.Add(cartDetails);
                    cart.cartDetails = _cartDetails;
                    cart.sessionId = sessionId;
                    cart.usersId = user.UserId;
                    _userApi.Createcart(cart);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddtoCart(int id, string sessionId)
        {
            CartDetails cartDetails = new CartDetails();
            MyCart cart = new MyCart();
            cartDetails.productId = id;
            cartDetails.quantity = 1;
            var productData = _opApi.GetAll().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
            cartDetails.price = 1 * productData.price;
            bool check = false;
            _cartDetails = new List<CartDetails>();
            string name = _distributedCache.GetStringAsync("cart").Result;
            _carts = new List<MyCart>();
            try
            {
                _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                if (_carts != null && _carts.Count()!=0)
                {
                    if (_carts.ToList().Any(c => c.sessionId.Equals(sessionId)))
                    {
                        foreach (var data in _carts)
                        {
                            if (data.sessionId.Equals(sessionId))
                            {
                                foreach (var data1 in data.cartDetails)
                                {
                                    if (data1.productId.Equals(id))
                                    {
                                        var product = _opApi.GetAll().Result.Where(c => c.id.Equals(data1.productId)).FirstOrDefault();
                                        data1.product = product;
                                        var quantity = data1.quantity;
                                        data1.quantity = quantity + 1;
                                        data1.price = data1.quantity * data1.product.price;
                                        check = true;
                                        /*cartList.Add(data1);*/
                                    }
                                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }
                        if (check == false)
                        {
                            _carts.Add(cart);
                        }
                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                    else
                    {
                        _carts.Add(cart);
                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }

                }
                else
                {
                    _carts = new List<MyCart>();
                    _cartDetails.Add(cartDetails);
                    cart.cartDetails = _cartDetails;
                    cart.sessionId = sessionId;
                    _carts.Add(cart);
                    _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                }

            }
            catch (Exception ex)
            {
                _cartDetails.Add(cartDetails);
                cart.cartDetails = _cartDetails;
                cart.sessionId = sessionId;
                _carts.Add(cart);
                _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));

            }
            return false;
        }

        public bool Quantity(int id, int quantity, UserRegistration user)
        {
            try
            {

                MyCart myCart = new MyCart();
                myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();

                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = quantity;
                        mycartData.price = quantity * mycartData.product.price;
                    }
                }
                _userApi.EditCart(myCart);
                return true;
            }
            catch (Exception ex)

            {
                return false;
            }
        }
        public bool Quantity(int id, int quantity, string sessionId)
        {
            try
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                if (_carts != null || _carts.Count > 0)
                {
                    if (_carts.ToList().Any(c => c.sessionId.Equals(sessionId)))
                    {
                        foreach (var data in _carts)
                        {
                            foreach (var caratDetailsData in data.cartDetails)
                            {
                                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                                caratDetailsData.product = product;
                            }
                            if (data.sessionId.Equals(sessionId))
                            {
                                foreach (var data1 in data.cartDetails)
                                {
                                    if (data1.productId.Equals(id))
                                    {
                                        data1.quantity = quantity;
                                        data1.price = data1.quantity * data1.product.price;
                                        /*cartList.Add(data1);*/
                                    }
                                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }

                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                }
                return true;
            }
            catch (Exception ex)

            {
                return false;
            }
        }

        public bool RemoveCart(int id, UserRegistration user)
        {
            try
            {
                try
                {
                    string name = JsonConvert.SerializeObject(_userApi.GetCart().Result);
                    if (JsonConvert.DeserializeObject<List<MyCart>>(name) != null)
                    {
                        _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                    }

                }
                catch (Exception ex)
                {
                }
                if (_carts.ToList().Where(c => c.usersId.Equals(user.UserId)) != null)
                {
                    var data = _carts.ToList().Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();

                    var count = 0;
                    foreach (var item in data.cartDetails.ToList())
                    {
                        if (item.productId.Equals(id))
                        {
                            data.cartDetails.Remove(item);
                        }

                    }
                    MyCart myCart = new MyCart();
                    myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                    foreach (var mycartData in myCart.cartDetails)
                    {
                        if (mycartData.productId.Equals(id))
                        {
                            _userApi.DeleteCartDetails(mycartData.id);
                        }
                    }
                    _userApi.EditCart(data);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<MyCart> CartFromDb()
        {
            try
            {
                string name = JsonConvert.SerializeObject(_userApi.GetCart().Result);
                if (JsonConvert.DeserializeObject<List<MyCart>>(name) != null)
                {
                    _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                }
                return _carts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<MyCart> CartFromSession()
        {
            try
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                if (JsonConvert.DeserializeObject<List<MyCart>>(name) != null)
                {
                    _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                }
                return _carts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Plus(int id, UserRegistration user)
        {
            try
            {
                MyCart myCart = new MyCart();
                myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                foreach (var caratDetailsData in myCart.cartDetails)
                {
                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                    caratDetailsData.product = product;
                }
                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = mycartData.quantity + 1;
                        mycartData.price = mycartData.quantity * mycartData.product.price;
                    }
                }
                var checkNegative = myCart.cartDetails.Where(c => c.price > 0);
                if (checkNegative != null)
                {
                    _userApi.EditCart(myCart);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Plus(int id, string sessionId)
        {
            try
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                if (_carts != null || _carts.Count > 0)
                {
                    if (_carts.ToList().Any(c => c.sessionId.Equals(sessionId)))
                    {
                        foreach (var data in _carts)
                        {
                            foreach (var caratDetailsData in data.cartDetails)
                            {
                                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                                caratDetailsData.product = product;
                            }
                            if (data.sessionId.Equals(sessionId))
                            {

                                foreach (var data1 in data.cartDetails)
                                {
                                    if (data1.productId.Equals(id))
                                    {

                                        data1.quantity = data1.quantity + 1;
                                        data1.price = data1.quantity * data1.product.price;


                                        /*cartList.Add(data1);*/
                                    }
                                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }

                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Minus(int id, UserRegistration user)
        {
            try
            {

                MyCart myCart = new MyCart();
                myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                foreach (var caratDetailsData in myCart.cartDetails)
                {
                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                    caratDetailsData.product = product;
                }
                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        if ((mycartData.quantity - 1) * mycartData.product.price > 0)
                        {
                            mycartData.quantity = mycartData.quantity - 1;
                            mycartData.price = mycartData.quantity * mycartData.product.price;
                        }
                    }
                }
                _userApi.EditCart(myCart);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Minus(int id, string sessionId)
        {
            try
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                if (_carts != null || _carts.Count > 0)
                {
                    if (_carts.ToList().Any(c => c.sessionId.Equals(sessionId)))
                    {
                        foreach (var data in _carts)
                        {
                            foreach (var caratDetailsData in data.cartDetails)
                            {
                                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                                caratDetailsData.product = product;
                            }
                            if (data.sessionId.Equals(sessionId))
                            {

                                foreach (var data1 in data.cartDetails)
                                {
                                    if (data1.productId.Equals(id))
                                    {
                                        if ((data1.quantity - 1) * data1.product.price > 0)
                                        {
                                            data1.quantity = data1.quantity - 1;
                                            data1.price = data1.quantity * data1.product.price;
                                        }
                                        /*cartList.Add(data1);*/
                                    }
                                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }

                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
