using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.Helper;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Manager.Interface;
using BT.CDMS.Business.Models;
using BT.CDMS.Business.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using Unity;
using BT.CDMS.Business.Exceptions;

namespace BT.CDMS.Business.Manager
{
    /// <summary>
    /// Class GridManager
    /// </summary>
    public class GridManager : IGridManager
    {
        #region Private Member
        private IGridDAO _gridDAO;
        private IGridDAO GridDAO
        {
            get
            {
                if (_gridDAO == null)
                    _gridDAO = UnityHelper.Container.Resolve<IGridDAO>();
                return _gridDAO;
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// GetGridTemplatesByOrgId
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<GridTemplate> GetGridTemplatesByOrgId(string orgId)
        {
            var gridTemplateList = new List<GridTemplate>();
            var gridTemplate = GridDAO.GetGridTemplatesByOrgId(orgId);
            if (gridTemplate.Tables.Count > 0)
            {
                if (gridTemplate.Tables[0] != null && gridTemplate.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in gridTemplate.Tables[0].Rows)
                    {
                        var gridTemplateDetail = new GridTemplate()
                        {
                            GridTemplateId = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_GRID_TEMPLATE_ID]),
                            GridTemplateName = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_GRID_TEMPLATE_NAME]),
                            Description = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_GRID_TEMPLATE_DESCRIPTION])
                        };
                        gridTemplateList.Add(gridTemplateDetail);
                    }
                }
            }
            return gridTemplateList;           
        }

        /// <summary>
        /// CheckIfTemplateIsAccessibleToListOfUsers
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CheckGridTemplateAccessResponse> CheckIfTemplateIsAccessibleToListOfUsers(CheckGridTemplateAccessRequest request)
        {
            if(string.IsNullOrEmpty(request.GridTemplateId))
            {
                throw new BusinessException(BusinessExceptionConstants.INVALID_GRIDTEMPLATEID_MESSAGE, BusinessExceptionConstants.INVALID_GRIDTEMPLATEID);
            }
            if(request.UserIdsList==null || request.UserIdsList.Count==0)
            {
                throw new BusinessException(BusinessExceptionConstants.INVALID_GRIDTEMPLATE_ACCESS_USERLISTS_MESSAGE, BusinessExceptionConstants.INVALID_GRIDTEMPLATE_ACCESS_USERLISTS);
            }
            var checkAccessList = new List<CheckGridTemplateAccessResponse>();
            var checkAccess = GridDAO.CheckIfTemplateIsAccessibleToListOfUsers(request);
            if (checkAccess.Tables.Count > 0)
            {
                if (checkAccess.Tables[0] != null && checkAccess.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in checkAccess.Tables[0].Rows)
                    {
                        var checkAccessDetail = new CheckGridTemplateAccessResponse()
                        {
                            UserId = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_USER_ID_ALIAS]),
                            IsAccess = DataAccessHelper.ConvertToBool(dr[1])
                        };
                        checkAccessList.Add(checkAccessDetail);
                    }
                }
            }
            return checkAccessList;
        }
        #endregion
    }
}