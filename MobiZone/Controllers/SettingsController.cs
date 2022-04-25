﻿/*using ApiLayer.Messages;
using ApiLayer.Models;
using AutoMapper;
using BusinessObjectLayer;
using BusinessObjectLayer.ProductOperations;
using DomainLayer;
using DomainLayer.ProductModel;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
 

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _context;
        IPrivacyOperation _privacyOperation;
     
        ResponseModel<ProductEntity> _response;
        IEnumerable<ProductEntity> _productDataList;
      
        PrivacyPolicy _privacyPolicy;
        IMessages _productMessages;
        IWebHostEnvironment _webHostEnvironment;
        IMapper _mapper;

        public SettingsController(ProductDbContext context, IPrivacyOperation privacyOperation,  IWebHostEnvironment web, IMapper mapper)
        {
            #region Object Assigning
            _context = context;
            _webHostEnvironment = web;
            _privacyOperation = privacyOperation;
         
            _response = new ResponseModel<ProductEntity>();
            
              _log = LogManager.GetLogger(typeof(ProductController));
            _productMessages = new ProductMessages(_webHostEnvironment);
            _mapper = mapper;
            #endregion
        }
       /* public ResponseModel<PrivacyPolicy> GetDetails(int id)
        {

        }
*/
  