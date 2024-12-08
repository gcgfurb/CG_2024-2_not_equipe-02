#define CG_DEBUG
#define CG_Gizmo      
#define CG_OpenGL      
// #define CG_OpenTK
// #define CG_DirectX      
// #define CG_Privado      

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using System.Collections.Generic;


namespace gcgcg
{
  public class Mundo : GameWindow
  {
    private static Objeto mundo = null;
    private char rotuloNovo = '?';
    private Objeto objetoSelecionado = null;
    private Objeto _cuboMenor = null;

    private readonly float[] _sruEixos =
    {
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f  /* Z+ */
    };

    private int _vertexBufferObjectFront_sruEixos;
    private int _vertexArrayObjectFront_sruEixos;

    private Shader _shaderBranca;
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderCiano;
    private Shader _shaderMagenta;
    private Shader _shaderAmarela;

    private readonly float[] _verticesFront =
        {
            // Position         Texture coordinates
             1.05f,  1.05f, 1.05f, 1.0f, 1.0f, // top right
             1.05f, -1.05f, 1.05f, 1.0f, 0.0f, // bottom right
            -1.05f, -1.05f, 1.05f, 0.0f, 0.0f, // bottom left
            -1.05f,  1.05f, 1.05f, 0.0f, 1.0f  // top left
        };

    private readonly float[] _verticesBack =
        {
          // Position         Texture coordinates
          -1.05f,  1.05f, -1.05f, 1.0f, 1.0f, // top right
          -1.05f, -1.05f, -1.05f, 1.0f, 0.0f, // bottom right
           1.05f, -1.05f, -1.05f, 0.0f, 0.0f, // bottom left
           1.05f,  1.05f, -1.05f, 0.0f, 1.0f  // top left
        };

    private readonly float[] _verticesTop =
{
    // Position         Texture coordinates
     1.05f,  1.05f, -1.05f, 1.0f, 1.0f, // top right
     1.05f,  1.05f,  1.05f, 1.0f, 0.0f, // bottom right
    -1.05f,  1.05f,  1.05f, 0.0f, 0.0f, // bottom left
    -1.05f,  1.05f, -1.05f, 0.0f, 1.0f  // top left
};

    private readonly float[] _verticesDown =
    {
    // Position         Texture coordinates
     1.05f, -1.05f,  1.05f, 1.0f, 1.0f, // top right
     1.05f, -1.05f, -1.05f, 1.0f, 0.0f, // bottom right
    -1.05f, -1.05f, -1.05f, 0.0f, 0.0f, // bottom left
    -1.05f, -1.05f,  1.05f, 0.0f, 1.0f  // top left
};

    private readonly float[] _verticesRight =
    {
    // Position         Texture coordinates
     1.05f,  1.05f, -1.05f, 1.0f, 1.0f, // top right
     1.05f, -1.05f, -1.05f, 1.0f, 0.0f, // bottom right
     1.05f, -1.05f,  1.05f, 0.0f, 0.0f, // bottom left
     1.05f,  1.05f,  1.05f, 0.0f, 1.0f  // top left
};

    private readonly float[] _verticesLeft =
    {
    // Position         Texture coordinates
    -1.05f,  1.05f,  1.05f, 1.0f, 1.0f, // top right
    -1.05f, -1.05f,  1.05f, 1.0f, 0.0f, // bottom right
    -1.05f, -1.05f, -1.05f, 0.0f, 0.0f, // bottom left
    -1.05f,  1.05f, -1.05f, 0.0f, 1.0f  // top left
};


    private readonly uint[] _indices =
    {
      0, 1, 3,
      1, 2, 3
    };

    private readonly Vector3[] _pointLightPositions =
        {
            new Vector3(0.7f, 0.2f, 2.0f),
            new Vector3(2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f, 2.0f, -12.0f),
            new Vector3(0.0f, 0.0f, -3.0f)
        };

    private int _elementBufferObjectFront;
    private int _vertexBufferObjectFront;
    private int _vertexArrayObjectFront;

    private int _vertexBufferObjectBack;
    private int _vertexArrayObjectBack;
    private int _elementBufferObjectBack;

    private int _vertexBufferObjectTop;
    private int _vertexArrayObjectTop;
    private int _elementBufferObjectTop;

    private int _vertexBufferObjectDown;
    private int _vertexArrayObjectDown;
    private int _elementBufferObjectDown;

    private int _vertexBufferObjectRight;
    private int _vertexArrayObjectRight;
    private int _elementBufferObjectRight;

    private int _vertexBufferObjectLeft;
    private int _vertexArrayObjectLeft;
    private int _elementBufferObjectLeft;

    private int _vaoModel;

    private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

    private Shader _shader;

    private Shader _basicLighting;
    private Shader _lightingMaps;
    private Shader _directionalLights;
    private Shader _pointLights;
    private Shader _spotlight;
    private Shader _multipleLights;

    private Shader _currentShader;

    private Texture _texture;
    private Texture _texture2;

    private readonly Vector3[] _cubePositions =
       {
            new Vector3(0.0f, 0.0f, 0.0f)
        };


    private Camera _camera;
    private bool _firstMove = true;
    private Vector2 _lastPos;
    private Vector3 _origin = new(0, 0, 0);

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloNovo); //padrão Singleton
    }


    protected override void OnLoad()
    {
      base.OnLoad();

      Utilitario.Diretivas();
#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif

      GL.Enable(EnableCap.DepthTest);

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      _shader = new Shader("Shaders/shaderTexture.vert", "Shaders/shaderTexture.frag");
      _shader.Use();

      _currentShader = _shader;

      _basicLighting = new Shader("Shaders/lighting.vert", "Shaders/lighting.frag");
      _lightingMaps = new Shader("Shaders/lmaps.vert", "Shaders/lmaps.frag");
      _directionalLights = new Shader("Shaders/dlights.vert", "Shaders/dlights.frag");
      _pointLights = new Shader("Shaders/plights.vert", "Shaders/plights.frag");
      _spotlight = new Shader("Shaders/spotlight.vert", "Shaders/spotlight.frag");
      _multipleLights = new Shader("Shaders/mlights.vert", "Shaders/mlights.frag");

      _vertexArrayObjectFront = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectFront);

      _vertexBufferObjectFront = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectFront);
      GL.BufferData(BufferTarget.ArrayBuffer, _verticesFront.Length * sizeof(float), _verticesFront, BufferUsageHint.StaticDraw);

      var vertexLocationFront = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocationFront);
      GL.VertexAttribPointer(vertexLocationFront, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocationFront = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocationFront);
      GL.VertexAttribPointer(texCoordLocationFront, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _elementBufferObjectFront = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjectFront);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _vertexArrayObjectBack = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectBack);

      _vertexBufferObjectBack = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectBack);
      GL.BufferData(BufferTarget.ArrayBuffer, _verticesBack.Length * sizeof(float), _verticesBack, BufferUsageHint.StaticDraw);

      var vertexLocationBack = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocationBack);
      GL.VertexAttribPointer(vertexLocationBack, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocationBack = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocationBack);
      GL.VertexAttribPointer(texCoordLocationBack, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _elementBufferObjectBack = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjectBack);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _vertexArrayObjectTop = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectTop);

      _vertexBufferObjectTop = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectTop);
      GL.BufferData(BufferTarget.ArrayBuffer, _verticesTop.Length * sizeof(float), _verticesTop, BufferUsageHint.StaticDraw);

      var vertexLocationTop = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocationTop);
      GL.VertexAttribPointer(vertexLocationTop, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocationTop = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocationTop);
      GL.VertexAttribPointer(texCoordLocationTop, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _elementBufferObjectTop = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjectTop);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _vertexArrayObjectDown = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectDown);

      _vertexBufferObjectDown = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectDown);
      GL.BufferData(BufferTarget.ArrayBuffer, _verticesDown.Length * sizeof(float), _verticesDown, BufferUsageHint.StaticDraw);

      var vertexLocationDown = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocationDown);
      GL.VertexAttribPointer(vertexLocationDown, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocationDown = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocationDown);
      GL.VertexAttribPointer(texCoordLocationDown, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _elementBufferObjectDown = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjectDown);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _vertexArrayObjectRight = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectRight);

      _vertexBufferObjectRight = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectRight);
      GL.BufferData(BufferTarget.ArrayBuffer, _verticesRight.Length * sizeof(float), _verticesRight, BufferUsageHint.StaticDraw);

      var vertexLocationRight = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocationRight);
      GL.VertexAttribPointer(vertexLocationRight, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocationRight = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocationRight);
      GL.VertexAttribPointer(texCoordLocationRight, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _elementBufferObjectRight = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjectRight);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _vertexArrayObjectLeft = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectLeft);

      _vertexBufferObjectLeft = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectLeft);
      GL.BufferData(BufferTarget.ArrayBuffer, _verticesLeft.Length * sizeof(float), _verticesLeft, BufferUsageHint.StaticDraw);

      var vertexLocationLeft = _shader.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocationLeft);
      GL.VertexAttribPointer(vertexLocationLeft, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordLocationLeft = _shader.GetAttribLocation("aTexCoord");
      GL.EnableVertexAttribArray(texCoordLocationLeft);
      GL.VertexAttribPointer(texCoordLocationLeft, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      _elementBufferObjectLeft = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjectLeft);
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

      _texture = Texture.LoadFromFile("Resources/R.jpg");
      _texture.Use(TextureUnit.Texture0);


      _shader.SetInt("texture0", 0);

      #region Cores
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      #endregion

      #region Eixos: SRU  
      _vertexBufferObjectFront_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjectFront_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObjectFront_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObjectFront_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);
      #endregion

      var verticesGrande = new Ponto4D[]
      {
        new Ponto4D(-1f, -1f,  1f),
        new Ponto4D( 1f, -1f,  1f),
        new Ponto4D( 1f,  1f,  1f),
        new Ponto4D(-1f,  1f,  1f),
        new Ponto4D(-1f, -1f, -1f),
        new Ponto4D( 1f, -1f, -1f),
        new Ponto4D( 1f,  1f, -1f),
        new Ponto4D(-1f,  1f, -1f)
      };

      var verticesMenor = new Ponto4D[]
     {
        new Ponto4D(-0.3, -0.3,  0.3),
        new Ponto4D( 0.3, -0.3,  0.3),
        new Ponto4D( 0.3,  0.3,  0.3),
        new Ponto4D(-0.3,  0.3,  0.3),
        new Ponto4D(-0.3, -0.3, -0.3),
        new Ponto4D( 0.3, -0.3, -0.3),
        new Ponto4D( 0.3,  0.3, -0.3),
        new Ponto4D(-0.3,  0.3, -0.3)
     };

      #region Objeto: Cubo
      objetoSelecionado = new Cubo(mundo, ref rotuloNovo);
      objetoSelecionado.shaderCor = _shaderAzul;
      #endregion

      #region Objeto: Cubo Menor
      _cuboMenor = new Cubo(mundo, ref rotuloNovo);
      _cuboMenor.shaderCor = _shaderBranca;
      _cuboMenor.MatrizEscalaXYZ(0.1, 0.1, 0.1);
      _cuboMenor.MatrizTranslacaoXYZ(3.0, 0.0, 0.0);
      #endregion

      _camera = new Camera(Vector3.UnitZ * 5, ClientSize.X / (float)ClientSize.Y);

      CursorState = CursorState.Grabbed;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      mundo.Desenhar(new Transformacao4D(), _camera);

      _texture.Use(TextureUnit.Texture0);
      //_texture2.Use(TextureUnit.Texture1);

      GL.BindVertexArray(_vaoModel);

      if (_currentShader == _basicLighting)
      {
        _basicLighting.Use();

        _basicLighting.SetMatrix4("model", Matrix4.Identity);
        _basicLighting.SetMatrix4("view", _camera.GetViewMatrix());
        _basicLighting.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _basicLighting.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
        _basicLighting.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
        _basicLighting.SetVector3("lightPos", _lightPos);
        _basicLighting.SetVector3("viewPos", _camera.Position);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

      if (_currentShader == _lightingMaps)
      {
        _lightingMaps.Use();

        _lightingMaps.SetMatrix4("model", Matrix4.Identity);
        _lightingMaps.SetMatrix4("view", _camera.GetViewMatrix());
        _lightingMaps.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _lightingMaps.SetVector3("viewPos", _camera.Position);

        _lightingMaps.SetInt("material.diffuse", 0);
        _lightingMaps.SetInt("material.specular", 1);
        _lightingMaps.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _lightingMaps.SetFloat("material.shininess", 32.0f);

        _lightingMaps.SetVector3("light.position", _lightPos);
        _lightingMaps.SetVector3("light.ambient", new Vector3(0.2f));
        _lightingMaps.SetVector3("light.diffuse", new Vector3(0.5f));
        _lightingMaps.SetVector3("light.specular", new Vector3(1.0f));

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
      }

      if (_currentShader == _directionalLights)
      {
        _directionalLights.Use();

        _directionalLights.SetMatrix4("model", Matrix4.Identity);
        _directionalLights.SetMatrix4("view", _camera.GetViewMatrix());
        _directionalLights.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _directionalLights.SetVector3("viewPos", _camera.Position);

        _directionalLights.SetInt("material.diffuse", 0);
        _directionalLights.SetInt("material.specular", 1);
        _directionalLights.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _directionalLights.SetFloat("material.shininess", 32.0f);

        _directionalLights.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
        _directionalLights.SetVector3("light.ambient", new Vector3(0.2f));
        _directionalLights.SetVector3("light.diffuse", new Vector3(0.5f));
        _directionalLights.SetVector3("light.specular", new Vector3(1.0f));

        for (int i = 0; i < _cubePositions.Length; i++)
        {
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          _directionalLights.SetMatrix4("model", model);

          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
      }

      if (_currentShader == _pointLights)
      {
        _pointLights.Use();

        _pointLights.SetMatrix4("model", Matrix4.Identity);
        _pointLights.SetMatrix4("view", _camera.GetViewMatrix());
        _pointLights.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _pointLights.SetVector3("viewPos", _camera.Position);

        _pointLights.SetInt("material.diffuse", 0);
        _pointLights.SetInt("material.specular", 1);
        _pointLights.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _pointLights.SetFloat("material.shininess", 32.0f);

        _pointLights.SetVector3("light.position", _lightPos);
        _pointLights.SetFloat("light.constant", 1.0f);
        _pointLights.SetFloat("light.linear", 0.09f);
        _pointLights.SetFloat("light.quadratic", 0.032f);
        _pointLights.SetVector3("light.ambient", new Vector3(0.2f));
        _pointLights.SetVector3("light.diffuse", new Vector3(0.5f));
        _pointLights.SetVector3("light.specular", new Vector3(1.0f));

        for (int i = 0; i < _cubePositions.Length; i++)
        {
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          _pointLights.SetMatrix4("model", model);

          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
      }

      if (_currentShader == _spotlight)
      {
        _spotlight.Use();

        _spotlight.SetMatrix4("view", _camera.GetViewMatrix());
        _spotlight.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _spotlight.SetVector3("viewPos", _camera.Position);

        _spotlight.SetInt("material.diffuse", 0);
        _spotlight.SetInt("material.specular", 1);
        _spotlight.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _spotlight.SetFloat("material.shininess", 32.0f);

        _spotlight.SetVector3("light.position", _camera.Position);
        _spotlight.SetVector3("light.direction", _camera.Front);
        _spotlight.SetFloat("light.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
        _spotlight.SetFloat("light.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
        _spotlight.SetFloat("light.constant", 1.0f);
        _spotlight.SetFloat("light.linear", 0.09f);
        _spotlight.SetFloat("light.quadratic", 0.032f);
        _spotlight.SetVector3("light.ambient", new Vector3(0.2f));
        _spotlight.SetVector3("light.diffuse", new Vector3(0.5f));
        _spotlight.SetVector3("light.specular", new Vector3(1.0f));

        for (int i = 0; i < _cubePositions.Length; i++)
        {
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          _spotlight.SetMatrix4("model", model);

          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
      }

      if (_currentShader == _multipleLights)
      {
        _multipleLights.Use();

        _multipleLights.SetMatrix4("view", _camera.GetViewMatrix());
        _multipleLights.SetMatrix4("projection", _camera.GetProjectionMatrix());

        _multipleLights.SetVector3("viewPos", _camera.Position);

        _multipleLights.SetInt("material.diffuse", 0);
        _multipleLights.SetInt("material.specular", 1);
        _multipleLights.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _multipleLights.SetFloat("material.shininess", 32.0f);

        _multipleLights.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
        _multipleLights.SetVector3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
        _multipleLights.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
        _multipleLights.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));

        for (int i = 0; i < _pointLightPositions.Length; i++)
        {
          _multipleLights.SetVector3($"pointLights[{i}].position", _pointLightPositions[i]);
          _multipleLights.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
          _multipleLights.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
          _multipleLights.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
          _multipleLights.SetFloat($"pointLights[{i}].constant", 1.0f);
          _multipleLights.SetFloat($"pointLights[{i}].linear", 0.09f);
          _multipleLights.SetFloat($"pointLights[{i}].quadratic", 0.032f);
        }

        _multipleLights.SetVector3("spotLight.position", _camera.Position);
        _multipleLights.SetVector3("spotLight.direction", _camera.Front);
        _multipleLights.SetVector3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
        _multipleLights.SetVector3("spotLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
        _multipleLights.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
        _multipleLights.SetFloat("spotLight.constant", 1.0f);
        _multipleLights.SetFloat("spotLight.linear", 0.09f);
        _multipleLights.SetFloat("spotLight.quadratic", 0.032f);
        _multipleLights.SetFloat("spotLight.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
        _multipleLights.SetFloat("spotLight.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));

        for (int i = 0; i < _cubePositions.Length; i++)
        {
          Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
          float angle = 20.0f * i;
          model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
          _multipleLights.SetMatrix4("model", model);

          GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
      }

      GL.BindVertexArray(_vertexArrayObjectFront);
      _texture.Use(TextureUnit.Texture0);
      _currentShader.Use();

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

      GL.BindVertexArray(_vertexArrayObjectBack);
      _texture.Use(TextureUnit.Texture0);
      _currentShader.Use();

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

      GL.BindVertexArray(_vertexArrayObjectTop);
      _texture.Use(TextureUnit.Texture0);
      _currentShader.Use();

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

      GL.BindVertexArray(_vertexArrayObjectDown);
      _texture.Use(TextureUnit.Texture0);
      _currentShader.Use();

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

      GL.BindVertexArray(_vertexArrayObjectRight);
      _texture.Use(TextureUnit.Texture0);
      _currentShader.Use();

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

      GL.BindVertexArray(_vertexArrayObjectLeft);
      _texture.Use(TextureUnit.Texture0);
      _currentShader.Use();

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

#if CG_Gizmo
      Gizmo_Sru3D();
#endif
      SwapBuffers();
    }


    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      _cuboMenor.MatrizRotacao(0.1);

      #region Teclado
      var estadoTeclado = KeyboardState;
      if (estadoTeclado.IsKeyDown(Keys.Escape))
        Close();

      if (estadoTeclado.IsKeyDown(Keys.D0))
        _currentShader = _shader;
      if (estadoTeclado.IsKeyDown(Keys.D1))
        _currentShader = _basicLighting;
      if (estadoTeclado.IsKeyDown(Keys.D2))
        _currentShader = _lightingMaps;
      if (estadoTeclado.IsKeyDown(Keys.D3))
        _currentShader = _directionalLights;
      if (estadoTeclado.IsKeyDown(Keys.D4))
        _currentShader = _pointLights;
      if (estadoTeclado.IsKeyDown(Keys.D5))
        _currentShader = _spotlight;
      if (estadoTeclado.IsKeyDown(Keys.D6))
        _currentShader = _multipleLights;

      const float cameraSpeed = 1.5f;
      var front = Vector3.Normalize(_origin - _camera.Position);
      var right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
      var up = Vector3.Normalize(Vector3.Cross(right, front));

      if (estadoTeclado.IsKeyDown(Keys.R))
        _camera.Position = Vector3.UnitZ * 5;
      if (estadoTeclado.IsKeyDown(Keys.W))
        _camera.Position += front * cameraSpeed * (float)e.Time; 
      if (estadoTeclado.IsKeyDown(Keys.S))
        _camera.Position -= front * cameraSpeed * (float)e.Time;
      if (estadoTeclado.IsKeyDown(Keys.A))
        _camera.Position -= right * cameraSpeed * (float)e.Time; 
      if (estadoTeclado.IsKeyDown(Keys.D))
        _camera.Position += right * cameraSpeed * (float)e.Time; 
      if (estadoTeclado.IsKeyDown(Keys.RightShift))
        _camera.Position += up * cameraSpeed * (float)e.Time;
      if (estadoTeclado.IsKeyDown(Keys.LeftShift))
        _camera.Position -= up * cameraSpeed * (float)e.Time;

      #endregion

      #region  Mouse

      #endregion

      var sensitivity = 1;
      const float rotationSpeed = 35f;
      var mouse = MouseState;

      if (MouseState.IsButtonDown(MouseButton.Left))
      {
        int janelaLargura = ClientSize.X;
        int janelaAltura = ClientSize.Y;
        Ponto4D mousePonto = new Ponto4D(MousePosition.X, MousePosition.Y);
        Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

        if (_firstMove)
        {

          _camera.Position = Vector3.UnitZ * 5;
          _lastPos = new Vector2((float)sruPonto.X, (float)sruPonto.Y);
          _firstMove = false;
        }
        else
        {
          var deltaX = sruPonto.X - _lastPos.X;
          var deltaY = sruPonto.Y - _lastPos.Y;
          _lastPos = new Vector2((float)sruPonto.X, (float)sruPonto.Y);

          _camera.Pitch -= rotationSpeed * -(float)sruPonto.Y * (float)e.Time;
          _camera.Position += _camera.Up * cameraSpeed * -(float)sruPonto.Y * (float)e.Time;

          _camera.Yaw -= rotationSpeed * (float)sruPonto.X * (float)e.Time;
          _camera.Position += _camera.Right * cameraSpeed * (float)1.9 * (float)sruPonto.X * (float)e.Time;
        }


      }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);

#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif
      GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    }

    protected override void OnUnload()
    {
      mundo.OnUnload();

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

      GL.DeleteBuffer(_vertexBufferObjectFront_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObjectFront_sruEixos);

      GL.DeleteProgram(_shaderBranca.Handle);
      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      GL.DeleteProgram(_shaderCiano.Handle);
      GL.DeleteProgram(_shaderMagenta.Handle);
      GL.DeleteProgram(_shaderAmarela.Handle);

      base.OnUnload();
    }

#if CG_Gizmo
    private void Gizmo_Sru3D()
    {
#if CG_OpenGL && !CG_DirectX
      var model = Matrix4.Identity;
      GL.BindVertexArray(_vertexArrayObjectFront_sruEixos);
      // Textura
      _shader.SetMatrix4("model", model);
      _shader.SetMatrix4("view", _camera.GetViewMatrix());
      _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shader.Use();
      // EixoX
      _shaderVermelha.SetMatrix4("model", model);
      _shaderVermelha.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVermelha.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.SetMatrix4("model", model);
      _shaderVerde.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVerde.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.SetMatrix4("model", model);
      _shaderAzul.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderAzul.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif    

  }
}
