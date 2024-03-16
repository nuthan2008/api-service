using Microsoft.AspNetCore.Mvc;
using BusinessProvider.Models;
using BusinessProvider.providers;

namespace SampleDotNetCoreApiProject.Models
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessLogicProvider _businessProvider;


        public BusinessController(IBusinessLogicProvider businessProvider)
        {
            _businessProvider = businessProvider;
        }

        [HttpPost]
        [Route("format")]
        public async Task<IActionResult> getFormattedData(string id, CancellationToken cancellationToken)
        {

            var response = await _businessProvider.getDataById(id, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> getDataRequest()
        {
            // try
            // {
            //     var requestId = req.requestId;

            //     // get request details
            //     var tenant = req.Params.Tenant;
            //     var types = req.Params.Types;
            //     var dataId = req.Params.Id;
            //     // get the userId from the security token
            //     // TODO log this in the logging
            //     // var userId = req.user.sub;
            //     var appName = req.Params.App;
            //     string subTenant = "";
            //     string subTenantLog = "null";
            //     // since the locale is optional it would
            //     // come as undefined
            //     string locale = "";
            //     string dataLocale = "";

            //     var svcName = req.Params.Service.ToLower();

            //     if (!String.IsNullOrEmpty(req.Params.Subtenant))
            //     {
            //         subTenant = req.Params.Subtenant;
            //         subTenantLog = subTenant;
            //     }
            //     else
            //     {
            //         subTenantLog = "-";
            //     }

            //     if (!String.IsNullOrEmpty(req.Params.Locale))
            //     {
            //         locale = req.Params.Locale;
            //         dataLocale = req.Params.DataLocale;
            //     }

            //     // logger.info(
            //     //     platformMessages.commonCore.CCI010.enUS.msg,
            //     //     req.correlationId,
            //     //     req.callerId,
            //     //     req.requestId,
            //     //     req.Params.app,
            //     //     req.Params.microApp,
            //     //     req.Params.service,
            //     //     req.Params.tenant,
            //     //     subTenantLog,
            //     //     req.Params.typesName,
            //     //     req.Params.dataLocale,
            //     //     req.user.sub,
            //     //     req.method
            //     // );

            //     //     let type = null;

            //     //     const typesConfig =
            //     //         PlatformService.AppsConfig.apps[appName].services[svcName].types[
            //     //             types
            //     //         ];

            //     //     if (typesConfig)
            //     //     {
            //     //         type = typesConfig.typeName;
            //     //         // sysDomainType = typesConfig.tenant;
            //     //     }
            //     //     else
            //     //     {
            //     //         const typeError = new Error(`Type ${ types } not supported`);
            //     //         typeError.request = true;
            //     //         typeError.status = 400;
            //     //         throw typeError;
            //     //     }

            //     //     const requestInstance =
            //     //         UtilityService.cloneOperationRequest(operationRequest);

            //     //     // Provide a requestId
            //     //     requestInstance.requestId = requestId;
            //     //     requestInstance.appName = appName;
            //     //     requestInstance.serviceName = svcName;

            //     //     requestInstance.params.id = dataId;
            //     //     requestInstance.params.type = type;
            //     //     requestInstance.params.tenant = tenant;
            //     //     requestInstance.params.subTenant = subTenant;
            //     //     requestInstance.params.locale = locale;
            //     //     requestInstance.params.dataLocale = dataLocale;

            //     //     const dataTechService = new DataTechnologyService();

            //     //     logger.debug(
            //     //         platformMessages.commonCore.CCD003.enUS.msg,
            //     //         requestInstance.requestId,
            //     //         req.callerId,
            //     //         req.requestId,
            //     //         req.params.app,
            //     //         req.params.microApp,
            //     //         req.params.service,
            //     //         req.params.tenant,
            //     //         subTenantLog,
            //     //         req.params.typesName,
            //     //         req.params.dataLocale,
            //     //         req.user.sub,
            //     //         req.method,
            //     //         requestInstance
            //     //     );

            //     //     // TODO do validation when no Id or Type is passed
            //     //     // Create the common operation for all types of get request
            //     //     let operationInstance = null;

            //     //     // If Id is present do get by id else get by type
            //     //     if (dataId)
            //     //     {
            //     //         operationInstance = await dataTechService.get(requestInstance);
            //     //     }
            //     //     else
            //     //     {
            //     //         operationInstance = await dataTechService.getByType(
            //     //             requestInstance
            //     //         );
            //     //     }

            //     //     logger.debug(
            //     //         platformMessages.commonCore.CCD004.enUS.msg,
            //     //         req.correlationId,
            //     //         req.callerId,
            //     //         requestInstance.requestId,
            //     //         req.params.app,
            //     //         req.params.microApp,
            //     //         req.params.service,
            //     //         req.params.tenant,
            //     //         subTenantLog,
            //     //         req.params.typesName,
            //     //         req.params.dataLocale,
            //     //         req.user.sub,
            //     //         req.method,
            //     //         operationInstance
            //     //     );

            //     //     // TODO Pass this through the app service
            //     //     // to determine what to do with the system data

            //     //     res.setHeader(
            //     //         platformConstants.contentType,
            //     //         platformConstants.contentTypeJson
            //     //     );

            //     //     // TODO Handle Error Response

            //     //     let responseCode = platformConstants.responseCode.ok;

            //     //     if (
            //     //         operationInstance.response.status ===
            //     //         platformConstants.status.notFound
            //     //     )
            //     //     {
            //     //         responseCode = platformConstants.responseCode.notFound;
            //     //     }

            //     //     // Set the header to ok or not found
            //     //     res.status(responseCode).send(operationInstance);

            //     //     logger.info(
            //     //         platformMessages.commonCore.CCI011.enUS.msg,
            //     //         req.correlationId,
            //     //         req.callerId,
            //     //         req.requestId,
            //     //         req.params.app,
            //     //         req.params.microApp,
            //     //         req.params.service,
            //     //         req.params.tenant,
            //     //         subTenantLog,
            //     //         req.params.typesName,
            //     //         req.params.dataLocale,
            //     //         req.user.sub,
            //     //         req.method
            //     //     );

            //     //     // TODO how to send the correct error message
            //     // }
            //     // catch (error)
            //     // {
            //     //     // do not throw, handle in the final handler
            //     //     errorHandler(error, req, res);
            //     // }

            // }
            // catch (Exception ex)
            // {
            //     // do not throw, handle in the final handler
            //     // errorHandler(error, req, res);
            // }
            // TODO Exactly the same method as RefData Service
            // Try to combine to common core
            return Ok("test data");
        }
    }
}