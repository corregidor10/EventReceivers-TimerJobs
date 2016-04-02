using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Lab05EventReceivers.EventReceiverFacturas
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiverFacturas : SPItemEventReceiver
    {

        private void UpdatePropertyBag(SPWeb web, double cambio)
        {

            string keyName = "TotalFacturas";
            double actual = 0;

            if (web.Properties[keyName]!=null)
            {
                actual = double.Parse(web.Properties[keyName]);
            }

            else
            {
                web.Properties.Add(keyName,"");
            }

            actual += cambio;

            web.Properties[keyName] = actual.ToString();

            web.Properties.Update();
        }

        public override void ItemAdding(SPItemEventProperties properties)
        {
            double valor;

            double.TryParse(properties.AfterProperties["Importe"].ToString(), out valor);

            UpdatePropertyBag(properties.Web, valor);
        }

       
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            double valorPrevio;

            double nuevoValor;

            double.TryParse(properties.ListItem["Importe"].ToString(), out valorPrevio);

            double.TryParse(properties.AfterProperties["Importe"].ToString(), out nuevoValor);

            double change = nuevoValor - valorPrevio;

            UpdatePropertyBag(properties.Web, change);


        }

      
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            double valor;

            double.TryParse(properties.ListItem["Importe"].ToString(), out valor);

            UpdatePropertyBag(properties.Web, -valor);
        }


    }
}