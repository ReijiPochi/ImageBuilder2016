// Tube3DMain.cpp : 実装ファイル
//

#include "stdafx.h"
#include <gl/glu.h> 
#define _USE_MATH_DEFINES
#include <math.h>


//#include "..\..\tube3d.h"
#include "..\..\tube3dFunc.h"
#include "Tube3DMain.h"
#include "..\OpenGL\GLDrawingtool3D.h"


// CTube3DMain



///--------------------------------------------------------------------
///コンストラクタ
///
///--------------------------------------------------------------------
CTube3DMain::CTube3DMain()
{

	m_WindowHandle = NULL;					//描画を行うコントロールのハンドル

	m_hDC = NULL;							//GDC
	m_hRC = NULL;							//OpenGLのコンテキスト

	mousestatus=0;
	leye=700.0;
	dip=0.65;
	rot=0.5; //dip,rot[rad]


}

///--------------------------------------------------------------------
///デストラクタ
///
///--------------------------------------------------------------------
CTube3DMain::~CTube3DMain()
{
	m_WindowHandle = NULL;					//描画を行うコントロールのハンドル

	m_hDC = NULL;							//GDC
	m_hRC = NULL;							//OpenGLのコンテキスト

}

///--------------------------------------------------------------------
///オープン
///
///--------------------------------------------------------------------
int CTube3DMain::Open()
{

	InitOpenGL();

	return ERRORCODE_TUBE3D_NON;
}

///--------------------------------------------------------------------
///クローズ
///
///--------------------------------------------------------------------
int CTube3DMain::Close()
{
	if (m_hDC != NULL)
	{
		DestroyOpenGL();
	}

	return ERRORCODE_TUBE3D_NON;
}


GLfloat light0pos[] = { 500.0, 1500.0, 500.0, 0.0 }; // x,y,z,d
GLfloat White[] = { 1.0, 1.0, 1.0, 1.0 }; // R,G,B,A
GLfloat Black[] = { 0.0, 0.0, 0.0, 1.0 }; // R,G,B,A
GLfloat Brown[] = { 0.3f, 0.25f, 0.0, 1.0 }; // R,G,B,A
GLfloat Cyan[] = { 0.0, 0.95f, 0.9f, 1.0 }; // R,G,B,A
GLfloat Gray[] = { 0.25f, 0.25f, 0.25f, 1.0 }; // R,G,B,A
GLfloat Red[] = { 1.0, 0.0, 0.0, 1.0 }; // R,G,B,A
GLfloat Shine[] = { 100.0}; //0.0(large highlight) ... 128.0(small highlight)


///--------------------------------------------------------------------
///描画主処理
///
///--------------------------------------------------------------------
int CTube3DMain::DrawData()
{

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	glEnable(GL_NORMALIZE); // normarize normal vectors

	double ddip=0,drot=0,dip1,rot1;
	double ly,lxz,lx,lz;
	if (mousestatus==2) {
		ddip=(posDrag.y-posDown.y)*0.01;
		drot=-(posDrag.x-posDown.x)*0.01;
	} else if (mousestatus==3) {
		ddip=(posUp.y-posDown.y)*0.01;
		drot=-(posUp.x-posDown.x)*0.01;
		dip+=ddip;
		rot+=drot;
		if (0.4999*M_PI<dip) dip=0.4999*M_PI;
		if (dip<-0.4999*M_PI) dip=-0.4999*M_PI;
		mousestatus=0;
		ddip=0,drot=0;
	}
	dip1=dip+ddip;
	rot1=rot+drot;
	if (0.4999*M_PI<dip1) dip1=0.4999*M_PI;
	if (dip1<-0.4999*M_PI) dip1=-0.4999*M_PI;
	ly=leye*sin(dip1); lxz=leye*cos(dip1);
	lx=lxz*sin(rot1); lz=lxz*cos(rot1);
	gluLookAt(lx,ly,lz, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	glLightfv(GL_LIGHT0, GL_AMBIENT, White);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, White);
	glLightfv(GL_LIGHT0, GL_SPECULAR, White);
	glLightfv(GL_LIGHT0, GL_POSITION, light0pos); //definition of light position must be here
	glClearColor(0.1f,0,0.2f,0);
	glClear(GL_COLOR_BUFFER_BIT);
	glClear(GL_DEPTH_BUFFER_BIT); // depth check
	glMaterialfv(GL_FRONT, GL_AMBIENT, White); // Attribute of the Axes
	drawAxes();

 	renderScene();//render here
	SwapBuffers(m_hDC);


	return ERRORCODE_TUBE3D_NON;
}

///--------------------------------------------------------------------
///マウスの座標を与える(Mouse Up)
///
///--------------------------------------------------------------------
int CTube3DMain::SetMousePosUp(int x, int y)
{
	if(mousestatus==2) {
		mousestatus=3;
		posUp.x = x;
		posUp.y = y;
	}

	TRACE("COpenGL::OnLButtonUp x,y=%d %d\n",x,y);
	return ERRORCODE_TUBE3D_NON;
}

///--------------------------------------------------------------------
///マウスの座標を与える(Mouse Down)
///
///--------------------------------------------------------------------
int CTube3DMain::SetMousePosDown(int x, int y)
{
	if(mousestatus==0) {
		mousestatus=1;
		posDown.x=x;
		posDown.y=y;
	}
	TRACE("COpenGL::OnLButtonDown x,y=%d %d\n",x,y);	
	return ERRORCODE_TUBE3D_NON;
}

///--------------------------------------------------------------------
///マウスの座標を与える(Mouse Move)
///
///--------------------------------------------------------------------
int CTube3DMain::SetMousePosMove(int x, int y)
{
	if (mousestatus > 0) {
		mousestatus=2;
		posDrag.x=x;
		posDrag.y=y;
		TRACE("COpenGL::OnMouseMove x,y=%d %d\n",x,y);
	}
	return ERRORCODE_TUBE3D_NON;
}


///--------------------------------------------------------------------
///Windowハンドルのセット
///
///--------------------------------------------------------------------
int CTube3DMain::set_WindowHandle(HANDLE h)				//ウインドウハンドルのセット
{
	m_WindowHandle = (HWND) h;
	return ERRORCODE_TUBE3D_NON;
}

int setPixelFormat(HDC hdc)
{
	PIXELFORMATDESCRIPTOR pfd = { 
	    sizeof(PIXELFORMATDESCRIPTOR),    // size of this pfd 
	    1,                                // version number 
	    PFD_DRAW_TO_WINDOW |              // support window 
	    PFD_SUPPORT_OPENGL |              // support OpenGL 
	    PFD_DOUBLEBUFFER,                 // double buffered 
	    PFD_TYPE_RGBA,                    // RGBA type 
	    24,                               // 24-bit color depth 
	    0, 0, 0, 0, 0, 0,                 // color bits ignored 
	    0,                                // no alpha buffer 
	    0,                                // shift bit ignored 
	    0,                                // no accumulation buffer 
	    0, 0, 0, 0,                       // accum bits ignored 
	    32,                               // 32-bit z-buffer     
	    0,                                // no stencil buffer 
	    0,                                // no auxiliary buffer 
	    PFD_MAIN_PLANE,                   // main layer 
	    0,                                // reserved 
	    0, 0, 0                           // layer masks ignored 
	}; 
	int  iPixelFormat; 
 
	// get the device context's best, available pixel format match 
	if((iPixelFormat = ChoosePixelFormat(hdc, &pfd)) == 0) {
		//MessageBox(NULL, "ChoosePixelFormat Failed", NULL, MB_OK);
		return 0;
	}
	 
	// make that match the device context's current pixel format 
	if(SetPixelFormat(hdc, iPixelFormat, &pfd) == FALSE) {
		//MessageBox(NULL, "SetPixelFormat Failed", NULL, MB_OK);
		return 0;
	}

	return 1;
}


void CTube3DMain::InitOpenGL()
{
	// TODO: この位置に固有の処理を追加するか、または基本クラスを呼び出してください
	CRect myRect;
	GetClientRect(m_WindowHandle, &myRect);
	TRACE("COpenGL::PreSubclassWindow RECT %d %d\n",myRect.Width(),myRect.Height());

	m_hDC = GetDC( m_WindowHandle );
	setPixelFormat(m_hDC); //**Kosaka added this line.**
	CreateRGBPalette();
	if (!(m_hRC = wglCreateContext(m_hDC))) exit(1); /*a2*/
	if(!wglMakeCurrent(m_hDC, m_hRC)) exit(1); /*a3*/

	glViewport(0,0,myRect.Width(),myRect.Height());
	glEnable(GL_DEPTH_TEST);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	const int angle=40;
	const int frontPlanePosZ=100;
	const int rearPlanePosZ=1300;
	gluPerspective(angle, (float)myRect.Width()/(float)myRect.Height(), frontPlanePosZ, rearPlanePosZ);
	initializeScene();
}

void CTube3DMain::DestroyOpenGL()
{
	TRACE("COpenGL::OnDestroy\n");
	wglMakeCurrent(NULL, NULL) ;  /*b1*/
	wglDeleteContext(m_hRC);   /*b2*/
	if (m_pOldPalette)
		SelectPalette(m_hDC, m_pOldPalette, FALSE);

	ReleaseDC(m_WindowHandle, m_hDC);

}

void CTube3DMain::CreateRGBPalette()
{
	static int defaultOverride[13] = {
		0, 3, 24, 27, 64, 67, 88, 173, 181, 236, 247, 164, 91
	};
	static PALETTEENTRY defaultPalEntry[20] = {
		{ 0,   0,   0,    0 },
		{ 0x80,0,   0,    0 },
		{ 0,   0x80,0,    0 },
		{ 0x80,0x80,0,    0 },
		{ 0,   0,   0x80, 0 },
		{ 0x80,0,   0x80, 0 },
		{ 0,   0x80,0x80, 0 },
		{ 0xC0,0xC0,0xC0, 0 },

		{ 192, 220, 192,  0 },
		{ 166, 202, 240,  0 },
		{ 255, 251, 240,  0 },
		{ 160, 160, 164,  0 },

		{ 0x80,0x80,0x80, 0 },
		{ 0xFF,0,   0,    0 },
		{ 0,   0xFF,0,    0 },
		{ 0xFF,0xFF,0,    0 },
		{ 0,   0,   0xFF, 0 },
		{ 0xFF,0,   0xFF, 0 },
		{ 0,   0xFF,0xFF, 0 },
		{ 0xFF,0xFF,0xFF, 0 }
	};

	PIXELFORMATDESCRIPTOR pfd;
	LOGPALETTE *pPal;
	int n, i;

	m_pOldPalette=NULL;

	n = ::GetPixelFormat(m_hDC);
	::DescribePixelFormat(m_hDC, n, sizeof(pfd), &pfd);

	if (pfd.dwFlags & PFD_NEED_PALETTE) {
		n = 1 << pfd.cColorBits;
		pPal = (PLOGPALETTE) new char[sizeof(LOGPALETTE) + n * sizeof(PALETTEENTRY)];

		ASSERT(pPal != NULL);

		pPal->palVersion = 0x300;
		pPal->palNumEntries = n;
		for (i=0; i<n; i++) {
			pPal->palPalEntry[i].peRed =
					ComponentFromIndex(i, pfd.cRedBits, pfd.cRedShift);
			pPal->palPalEntry[i].peGreen =
					ComponentFromIndex(i, pfd.cGreenBits, pfd.cGreenShift);
			pPal->palPalEntry[i].peBlue =
					ComponentFromIndex(i, pfd.cBlueBits, pfd.cBlueShift);
			pPal->palPalEntry[i].peFlags = 0;
		}

		/* fix up the palette to include the default GDI palette */
		if ((pfd.cColorBits == 8)                           &&
			(pfd.cRedBits   == 3) && (pfd.cRedShift   == 0) &&
			(pfd.cGreenBits == 3) && (pfd.cGreenShift == 3) &&
			(pfd.cBlueBits  == 2) && (pfd.cBlueShift  == 6)
		   ) {
			for (i = 1 ; i <= 12 ; i++)
				pPal->palPalEntry[defaultOverride[i]] = defaultPalEntry[i];
		}

		m_cPalette.CreatePalette(pPal);
		delete pPal;

		HPALETTE hp = (HPALETTE)m_cPalette;

		m_pOldPalette = SelectPalette(m_hDC, hp, FALSE);
		RealizePalette(m_hDC);

		//m_pOldPalette = m_pDC->SelectPalette(&m_cPalette, FALSE);
		//m_pDC->RealizePalette();
	}
}


void CTube3DMain::initializeScene()
{
	tick=0;
}

unsigned char CTube3DMain::ComponentFromIndex(int i, UINT nbits, UINT shift)
{
	unsigned char threeto8[8] = {
		0, 0111>>1, 0222>>1, 0333>>1, 0444>>1, 0555>>1, 0666>>1, 0377
	};
	unsigned char twoto8[4] = {
		0, 0x55, 0xaa, 0xff
	};
	unsigned char oneto8[2] = {
		0, 255
	};
	unsigned char val = (unsigned char) (i >> shift);
	switch (nbits) {
	case 1:
		val &= 0x1;
		return oneto8[val];
	case 2:
		val &= 0x3;
		return twoto8[val];
	case 3:
		val &= 0x7;
		return threeto8[val];
	default:
		return 0;
	}
}


polyhedron_t cube={
	8, //number of vertices of the cube
	{
		{50,50,50},{50,50,-50},{-50,50,-50},{-50,50,50},
		{50,-50,50},{50,-50,-50},{-50,-50,-50},{-50,-50,50}
	}, // coordinates of the vertices of the cube
	6, // number of faces of the cube
	{
		{4,{0,1,2,3},{0,1,0}}, {4,{0,4,5,1},{1,0,0}},
		{4,{1,5,6,2},{0,0,-1}}, {4,{2,6,7,3},{-1,0,0}},
		{4,{3,7,4,0},{0,0,1}}, {4,{7,6,5,4},{0,-1,0}}
	} // every face has four vertices and showing vertex numbers
};

void CTube3DMain::renderScene(void)
{
	glShadeModel(GL_SMOOTH);

	float k1=tick/200.f;
	glRotatef(k1*360.0f, 0.0, 1.0, 0.0); // Matrix A
	glTranslatef(200.0f, 0.0, 0.0);      // Matrix B
	glRotatef(k1*360.0f, 0.0, 0.0, 1.0); // Matrix C
	// Then Transformation Matrix for the cube is A*B*C

	glMaterialfv(GL_FRONT, GL_AMBIENT, Gray); //Attribute of the cube
	glMaterialfv(GL_FRONT, GL_DIFFUSE, Cyan);
	glMaterialfv(GL_FRONT, GL_SPECULAR, White);
	glMaterialfv(GL_FRONT, GL_SHININESS, Shine);
	putFlatPolyhedron(cube);

	tick++;
}






// CTube3DMain メンバ関数
