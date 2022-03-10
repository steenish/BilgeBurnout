Shader "Custom/WaveTest"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BumpLevel ("Bump level", Range(0,1)) = 1.0
        _TextureSize ("Texture size", Range(0,1000)) = 1000
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _BumpLevel;
        float _TextureSize;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float3 height2normal_sobel(float3x3 c) {      
            float3x3 x = float3x3(   1.0, 0.0, -1.0,
                                            2.0, 0.0, -2.0,
                                            1.0, 0.0, -1.0  );
        
            float3x3 y = float3x3(   1.0,  2.0,  1.0,
                                            0.0,  0.0,  0.0,
                                        -1.0, -2.0, -1.0 );
        
            x = x * c;
            y = y * c;
        
            float cx =  x[0][0] +x[0][2]
                        +x[1][0] +x[1][2]
                        +x[2][0] +x[2][2];
        
            float cy =  y[0][0] +y[0][1] +y[0][2]
                        +y[2][0] +y[2][1] +y[2][2];
                    
            float cz =  sqrt(1-(cx*cx+cy*cy));
        
            return float3(cx, cy, cz);
        }

        float3x3 img3x3(sampler2D color_map, float2 tc, float ts, int ch) {
            float   d = 1.0/ts; // ts, texture sampling size
            float3x3 c;
            c[0][0] = tex2D(color_map,tc + float2(-d,-d))[ch];
            c[0][1] = tex2D(color_map,tc + float2( 0,-d))[ch];
            c[0][2] = tex2D(color_map,tc + float2( d,-d))[ch]; 
            
            c[1][0] = tex2D(color_map,tc + float2(-d, 0))[ch];
            c[1][1] = tex2D(color_map,tc                )[ch];
            c[1][2] = tex2D(color_map,tc + float2( d, 0))[ch];
            
            c[2][0] = tex2D(color_map,tc + float2(-d, d))[ch];
            c[2][1] = tex2D(color_map,tc + float2( 0, d))[ch];
            c[2][2] = tex2D(color_map,tc + float2( d, d))[ch];
        
            return c;
        }

        float3x3 myimg3x3(float2 uv, float textureSampleRate) {
            float   d = 1.0 / textureSampleRate;
            float3x3 c;
            float time = _Time.w;
            float frequencyFactor = 50;
            c[0][0] = sin(frequencyFactor * (uv + float2(-d,-d)) + time);
            c[0][1] = sin(frequencyFactor * (uv + float2( 0,-d)) + time);
            c[0][2] = sin(frequencyFactor * (uv + float2( d,-d)) + time); 
            
            c[1][0] = sin(frequencyFactor * (uv + float2(-d, 0)) + time);
            c[1][1] = sin(frequencyFactor * uv + time);
            c[1][2] = sin(frequencyFactor * (uv + float2( d, 0)) + time);
            
            c[2][0] = sin(frequencyFactor * (uv + float2(-d, d)) + time);
            c[2][1] = sin(frequencyFactor * (uv + float2( 0, d)) + time);
            c[2][2] = sin(frequencyFactor * (uv + float2( d, d)) + time);
        
            return c;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            // float3x3 c = img3x3(color_map, IN.texcoord, texture_size, 0 );// red only
            // float3 normal = height2normal_sobel(c);
            // normal = normalize(float3(normal.xy, normal.z * bump_level));
            float3x3 height = myimg3x3(IN.uv_MainTex, _TextureSize);
            float3 normal = height2normal_sobel(height);
            normal = normalize(float3(normal.xy, normal.z * _BumpLevel));
            o.Normal = normal;
            // o.Albedo = lerp(o.Albedo, fixed4(1,1,1,1), 0.1 * height[1][1]);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
