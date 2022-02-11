Shader "Sprites/SpriteDistorted"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_angle ("Angle" , Range(0,180))=60
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			uniform float _angle;

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(IN.vertex);
				o.texcoord = IN.texcoord;
				o.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (o.vertex);
				#endif

				 //在MVP变换之后再进行旋转操作,并修改顶点的Z值(深度)
                //弧度
                fixed radian = _angle / 180 * 3.14159;
                fixed cosTheta = cos(radian);
                fixed sinTheta = sin(radian);

                //旋转中心点(测试用的四边形, 正常的spine做的模型脚下旋转的点就是(0,0), 可以省去下面这一步已经旋转完成后的 +center操作)
                half2 center = half2(0, -0);
                IN.vertex.zy -= center;

                half z = IN.vertex.z * cosTheta - IN.vertex.y * sinTheta;
                half y = IN.vertex.z * sinTheta + IN.vertex.y * cosTheta;
                IN.vertex = half4(IN.vertex.x, y, z, IN.vertex.w);

                IN.vertex.zy += center;

                float4 verticalClipPos = UnityObjectToClipPos(IN.vertex);
                o.vertex.z = verticalClipPos.z / verticalClipPos.w * o.vertex.w ;

				return o;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}