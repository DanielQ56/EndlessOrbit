using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


//Used the Unity Tutorial on In-App Purchases as reference 
public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public static IAPManager instance;


    private string removeAds = "remove_ads";

    void Awake()
    {
        instance = this;
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }


    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(removeAds, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void RemoveAds()
    {
        Debug.Log("Hello!");
        BuyProductID(removeAds);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if(string.Equals(args.purchasedProduct.definition.id, removeAds, System.StringComparison.Ordinal))
        {
            Debug.Log("Remove Ads Successful");
            GoogleAds.instance.SetAdBool(false);
        }
        else
        {
            Debug.Log("Unsuccessful");
        }
        return PurchaseProcessingResult.Complete;
    }
    
    void BuyProductID(string productId)
    {
        if(IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if(product != null && product.availableToPurchase)
            {
                if (string.Equals(productId, removeAds, System.StringComparison.Ordinal))
                {
                    if(GoogleAds.instance.ShouldShowAds())
                    {
                        m_StoreController.InitiatePurchase(product);
                    }
                    else
                    {
                        Debug.Log("Already bought");
                    }
                }
                else
                {
                    m_StoreController.InitiatePurchase(product);
                    Debug.Log("Successful");
                }
            }
            else
            {
                Debug.Log("Failed");
            }
        }
        else
        {
            Debug.Log("Not initialized!");
        }
    }

    public void RestorePurchases()
    {
        if(!IsInitialized())
        {
            return;
        }

        if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("Restoring on apple");
            });
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;

        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Failure Reason: " + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }


}
