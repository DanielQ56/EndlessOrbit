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
        BuyProductID(removeAds);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        Debug.Log("Processing the purchase");
        ScoreManager.instance.Loading(false);
        m_StoreController.ConfirmPendingPurchase(args.purchasedProduct);
#if UNITY_EDITOR
        GoogleAds.instance.SetAdBool(false);
        ScoreManager.instance.ProvideInfo("Purchase Successful!");
#else
        if (string.Equals(args.purchasedProduct.definition.id, removeAds, System.StringComparison.Ordinal))
        {
            if(GoogleAds.instance.ShouldShowAds())
            {
                GoogleAds.instance.SetAdBool(false);
                ScoreManager.instance.ProvideInfo("Purchase Successful!");
            }
            else
            {
                ScoreManager.instance.ProvideInfo("Purchase Restored!");
            }
        }
        else
        {
            ScoreManager.instance.ProvideInfo("Purchase Unsuccessful, Try Again.");
        }
#endif
        return PurchaseProcessingResult.Complete;
    }
    
    void BuyProductID(string productId)  
    {
        InitializePurchasing();

        if(IsInitialized())
        {
            ScoreManager.instance.Loading(true);
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
                        ScoreManager.instance.Loading(false);
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
                ScoreManager.instance.Loading(false);
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
        ScoreManager.instance.Loading(false);
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        ScoreManager.instance.ProvideInfo(string.Format("Purchase Failed: {0}", failureReason));
    }


}
