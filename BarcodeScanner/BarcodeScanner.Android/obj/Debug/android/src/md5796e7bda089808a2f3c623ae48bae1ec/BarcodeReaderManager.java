package md5796e7bda089808a2f3c623ae48bae1ec;


public class BarcodeReaderManager
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.honeywell.aidc.AidcManager.CreatedCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreated:(Lcom/honeywell/aidc/AidcManager;)V:GetOnCreated_Lcom_honeywell_aidc_AidcManager_Handler:Com.Honeywell.Aidc.AidcManager/ICreatedCallbackInvoker, DataCollectionLib\n" +
			"";
		mono.android.Runtime.register ("Honeywell.AIDC.CrossPlatform.BarcodeReaderManager, Honeywell.AIDC.CrossPlatform.BarcodeReader, Version=1.20.3.48, Culture=neutral, PublicKeyToken=null", BarcodeReaderManager.class, __md_methods);
	}


	public BarcodeReaderManager () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BarcodeReaderManager.class)
			mono.android.TypeManager.Activate ("Honeywell.AIDC.CrossPlatform.BarcodeReaderManager, Honeywell.AIDC.CrossPlatform.BarcodeReader, Version=1.20.3.48, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public BarcodeReaderManager (android.content.Context p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == BarcodeReaderManager.class)
			mono.android.TypeManager.Activate ("Honeywell.AIDC.CrossPlatform.BarcodeReaderManager, Honeywell.AIDC.CrossPlatform.BarcodeReader, Version=1.20.3.48, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onCreated (com.honeywell.aidc.AidcManager p0)
	{
		n_onCreated (p0);
	}

	private native void n_onCreated (com.honeywell.aidc.AidcManager p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
