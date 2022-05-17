package com.example.anticovid;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.android.material.tabs.TabLayout;

import org.json.JSONException;
import org.json.JSONObject;

import java.security.SecureRandom;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.X509TrustManager;


public class LoginTabFragment extends Fragment{

    EditText user,pass;
    TextView forgetPass;
    Button login;
    SharedPreferences sp;
    float v=0;
    private static  String URL_Login= "https://192.168.1.116:44322/api/Login?";


    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        ViewGroup root = (ViewGroup) inflater.inflate(R.layout.login_tab_fragment,container,false);



        user = root.findViewById(R.id.logUsername);
        pass = root.findViewById(R.id.logPass);
        forgetPass = root.findViewById(R.id.forget_pass);
        login = root.findViewById(R.id.loginButton);

        user.setTranslationX(800);
        pass.setTranslationX(800);
        forgetPass.setTranslationX(800);
        login.setTranslationX(800);

        user.setAlpha(v);
        pass.setAlpha(v);
        forgetPass.setAlpha(v);
        login.setAlpha(v);

        user.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        pass.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        forgetPass.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();
        login.animate().translationX(0).alpha(1).setDuration(800).setStartDelay(300).start();

        sp = getContext().getSharedPreferences("MyUserPrefs", Context.MODE_PRIVATE);

        login.setOnClickListener(view -> {


            try {
                //first getting the values
                String username = user.getText().toString();
                String password = pass.getText().toString();
                //validating inputs
                if(TextUtils.isEmpty(username)){
                    user.setError("Please enter your username");
                    user.requestFocus();
                    return;
                }

                if(TextUtils.isEmpty(password)){
                    pass.setError("Please enter your password");
                    pass.requestFocus();
                    return;
                }

                trustEveryone();
                URL_Login = "https://192.168.1.116:44322/api/Login?username="+username+"&password="+password;

                StringRequest stringRequest = new StringRequest(Request.Method.GET, URL_Login,
                        new Response.Listener<String>() {
                            @Override
                            public void onResponse(String response) {

                                try {
                                    //converting response to json object

                                    JSONObject obj = new JSONObject(response);
                                    Log.d("my", response.toString());

                                        //Log.d("A: ", obj.getString("Name"));
                                        //Log.d("B: ", obj.getString("Password"));
                                        String uName = obj.getString("Name");
                                        String uAddress= obj.getString("Address");
                                        String uMobile= obj.getString("Mobile");
                                        String uDOB= obj.getString("DateOfBirth");
                                        String uPass= obj.getString("Password");


                                        SharedPreferences.Editor editor = sp.edit();
                                        editor.putString("uName",uName);
                                        editor.putString("uAddress",uAddress);
                                        editor.putString("uMobile",uMobile);
                                        editor.putString("uDOB",uDOB);
                                        editor.putString("uPass",uPass);
                                        editor.commit();
                                        Toast.makeText(getActivity(),"Information Saved",Toast.LENGTH_SHORT).show();






                                        //getting the user from response
                                    //JSONObject userJson = obj.getJSONObject("");
                                    //for (int i=0;i<=userJson.length();i++)
                                    //{
                                     //   userJson.getString("Name");
                                      //  userJson.getString("Password");
                                  //  }

                                    //creating a new user object
                                   // User user = new User(
                                           // userJson.getString("Name"),
                                           // userJson.getString("Address"),
                                            //userJson.getString("Mobile"),
                                           // userJson.getString("DateOfBirth"),
                                           // userJson.getString("Password"),
                                            //false

                                    //);

                                    Intent intent = new Intent(getContext(), HomeView.class);
                                    startActivity(intent);




                                }
                                catch (JSONException e) {
                                    Log.e("Login", "onResponse: "+e);
                                    e.printStackTrace();
                                }
                            }
                        },
                        new Response.ErrorListener(){
                    @Override
                    public void onErrorResponse(VolleyError error) {

                        Toast.makeText(getContext(), error.getMessage(), Toast.LENGTH_SHORT).show();

                    }
                });
                RequestQueue requestQueue = Volley.newRequestQueue(getActivity());
                requestQueue.add(stringRequest);






            }
            catch (Exception e) {
                e.printStackTrace();
            }






    });
        return root;
}

    private void trustEveryone() {
        try {
            HttpsURLConnection.setDefaultHostnameVerifier(new HostnameVerifier(){
                public boolean verify(String hostname, SSLSession session) {
                    return true;
                }});
            SSLContext context = SSLContext.getInstance("TLS");
            context.init(null, new X509TrustManager[]{new X509TrustManager(){
                public void checkClientTrusted(X509Certificate[] chain,
                                               String authType) throws CertificateException {}
                public void checkServerTrusted(X509Certificate[] chain,
                                               String authType) throws CertificateException {}
                public X509Certificate[] getAcceptedIssuers() {
                    return new X509Certificate[0];
                }}}, new SecureRandom());
            HttpsURLConnection.setDefaultSSLSocketFactory(
                    context.getSocketFactory());
        } catch (Exception e) { // should never happen
            e.printStackTrace();
        }
    }

}
