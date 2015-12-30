#include "stdafx.h"
#include <math.h>
#include <GL/glu.h>
#include "GLDrawingtool3D.h"

///////////////// matrices and vectors ////////////////
matrix3D_t createIdentity(void)
{
	matrix3D_t u;
	int i,j;
	for (i=0;i<4;i++) {
		for(j=0;j<4;j++) u.m[i][j]=0.;
		u.m[i][i]=1.;
	}
	return u;
}

matrix3D_t operator * (matrix3D_t a,matrix3D_t b)
{
	matrix3D_t c;//c=a*b
	int i,j,k;
	for (i=0;i<4;i++) for (j=0;j<4;j++) {
		c.m[i][j]=0;
		for (k=0;k<4;k++) c.m[i][j]+=a.m[i][k]*b.m[k][j];
	}
	return c;
}

vector3D_t operator * (matrix3D_t a, vector3D_t b)
{
	vector3D_t c;//c=a*b
	int i,j;
	for (i=0;i<4;i++) {
		c.v[i]=0;
		for (j=0;j<4;j++) c.v[i]+=a.m[i][j]*b.v[j];
	}
	return c;
}

matrix3D_t translationMTX(float dx,float dy,float dz)
{
	matrix3D_t trans=createIdentity();
	trans.m[0][3]=dx;
	trans.m[1][3]=dy;
	trans.m[2][3]=dz;
	return trans;
}

matrix3D_t rotationXMTX(float theta)
{
	matrix3D_t rotate=createIdentity();
	float cs=(float)cos(theta);
	float sn=(float)sin(theta);
	rotate.m[1][1]=cs; rotate.m[1][2]=-sn;
	rotate.m[2][1]=sn; rotate.m[2][2]=cs;
	return rotate;
}

matrix3D_t rotationYMTX(float theta)
{
	matrix3D_t rotate=createIdentity();
	float cs=(float)cos(theta);
	float sn=(float)sin(theta);
	rotate.m[2][2]=cs; rotate.m[2][0]=-sn;
	rotate.m[0][2]=sn; rotate.m[0][0]=cs;
	return rotate;
}

matrix3D_t rotationZMTX(float theta)
{
	matrix3D_t rotate=createIdentity();
	float cs=(float)cos(theta);
	float sn=(float)sin(theta);
	rotate.m[0][0]=cs; rotate.m[0][1]=-sn;
	rotate.m[1][0]=sn; rotate.m[1][1]=cs;
	return rotate;
}

matrix3D_t scalingMTX(float factorx,float factory,float factorz)
{
	matrix3D_t scale=createIdentity();
	scale.m[0][0]=factorx;
	scale.m[1][1]=factory;
	scale.m[2][2]=factorz;
	return scale;
}

vector3D_t Point2Vector(point3D_t pnt)
{
	vector3D_t vec;
	vec.v[0]=pnt.x;
	vec.v[1]=pnt.y;
	vec.v[2]=pnt.z;
	vec.v[3]=1.;
	return vec;
}

point3D_t Vector2Point(vector3D_t vec)
{
	point3D_t pnt;
	pnt.x=vec.v[0];
	pnt.y=vec.v[1];
	pnt.z=vec.v[2];
	return pnt;
}

vector3D_t unitVector(vector3D_t vec)
{
	int i;
	float vec2=0.;
	float vec1,invvec1;
	for (i=0;i<3;i++) {
		vec2+=vec.v[i]*vec.v[i];
	}
	vec1=(float)sqrt(vec2);
	if (vec1!=0.) {
		invvec1=1/vec1;
		for (i=0;i<3;i++) {
			vec.v[i]*=invvec1;
		}
	}
	vec.v[3]=1.;
	return vec;
}

// inner product (dot product) of homogeneous vector
float operator * (vector3D_t a, vector3D_t b)
{
	float c;//c=a*b
	int i;
	c=0;
	for (i=0;i<3;i++) {
		c+=a.v[i]*b.v[i];
	}
	return c;
}

// outer product (cross product ) of homogeneous vector
//       i         j         k
//       a0       a1        a2
//       b0       b1        b2
vector3D_t operator ^ (vector3D_t a, vector3D_t b)
{
	vector3D_t c;//c=a*b
	c.v[0]=a.v[1]*b.v[2]-a.v[2]*b.v[1];
	c.v[1]=a.v[2]*b.v[0]-a.v[0]*b.v[2];
	c.v[2]=a.v[0]*b.v[1]-a.v[1]*b.v[0];
	c.v[3]=1.;
	return c;
}

vector3D_t operator - (vector3D_t v1,vector3D_t v0)
{
	vector3D_t c;//c=v1-v0
	c.v[0]=v1.v[0]-v0.v[0];
	c.v[1]=v1.v[1]-v0.v[1];
	c.v[2]=v1.v[2]-v0.v[2];
	c.v[3]=1.;
	return c;
}

vector3D_t operator - (vector3D_t v)
{
	vector3D_t c;//c=-v
	c.v[0]=-v.v[0];
	c.v[1]=-v.v[1];
	c.v[2]=-v.v[2];
	c.v[3]=1.;
	return c;
}

vector3D_t operator * (float r, vector3D_t b)
{
	vector3D_t c;//c=r*b
	int i;
	for (i=0;i<3;i++) {
		c.v[i]=r*b.v[i];
	}
	c.v[3]=1.;
	return c;
}

vector3D_t operator * (vector3D_t b, float r)
{
	vector3D_t c;//c=r*b
	int i;
	for (i=0;i<3;i++) {
		c.v[i]=r*b.v[i];
	}
	c.v[3]=1.;
	return c;
}

///////////// End of matrices and vectors /////////////

////////////// OpenGL drawShape Functions ver 3 /////////////////
void setColor(float red,float green,float blue)
{
	glColor3f(red, green, blue);	
}

void setColor(color_t col)
{
	glColor3f(col.r, col.g, col.b);	
}

void drawDot(float x,float y, float z)
{
	glBegin(GL_POINTS);
		glVertex3f(x, y, z);
	glEnd();
}

void drawLine(float x1, float y1, float z1, float x2, float y2, float z2)
{
	glBegin(GL_LINES);
		glVertex3f(x1, y1, z1);
		glVertex3f(x2, y2, z2);
	glEnd();
}

void drawLine(point3D_t p1,point3D_t p2)
{
	drawLine(p1.x,p1.y,p1.z,p2.x,p2.y,p2.z);
}

//n: number of points
void drawPolyline(point3D_t pnt[],int n)
{
	int i;
	glBegin(GL_LINE_STRIP);
		for (i=0;i<n;i++) {
			glVertex3f(pnt[i].x, pnt[i].y, pnt[i].z);
		}
	glEnd();
}

//n: number of vertices
void drawPolygon(point3D_t pnt[],int n)
{
	int i;
	glBegin(GL_LINE_LOOP);
		for (i=0;i<n;i++) {
			glVertex3f(pnt[i].x, pnt[i].y, pnt[i].z);
		}
	glEnd();
}

//n: number of vertices
void fillPolygon(point3D_t pnt[],int n,color_t color)
{
	int i;
	setColor(color);
	glBegin(GL_POLYGON);
		for (i=0;i<n;i++) {
			glVertex3f(pnt[i].x, pnt[i].y, pnt[i].z);
		}
	glEnd();
}

//n: number of vertices
void gradatePolygon(point3D_t pnt[],color_t col[],int num)
{
	int i;
	glBegin(GL_POLYGON);
		for (i=0;i<num;i++) {
			setColor(col[i]);
			glVertex3f(pnt[i].x, pnt[i].y, pnt[i].z);
		}
	glEnd();
}

//num: number of vertices
//NVector: normal vector to the surface of the polygon 
void putFlatPolygon(point3D_t pnt[],int num,vector3D_t NVector)
{
	int i;
	glBegin(GL_POLYGON);
		glNormal3f(NVector.v[0],NVector.v[1],NVector.v[2]);
		for (i=0;i<num;i++) {
			glVertex3f(pnt[i].x, pnt[i].y, pnt[i].z);
		}
	glEnd();
}

//num: number of vertices
//NVector: normal vectors to the surface of the polygon at the vertices
void putSmoothPolygon(point3D_t pnt[],vector3D_t NVector[],int num)
{
	int i;
	glBegin(GL_POLYGON);
		for (i=0;i<num;i++) {
			glNormal3f(NVector[i].v[0],NVector[i].v[1],NVector[i].v[2]);
			glVertex3f(pnt[i].x, pnt[i].y, pnt[i].z);
		}
	glEnd();
}

void putFlatPolyhedron(polyhedron_t &ph)
{
	point3D_t pntbuff[32];
	int i,j;
	for (i=0;i<ph.NumberofFaces;i++) {
		for (j=0;j<ph.fc[i].NumberofVertices;j++) {
			pntbuff[j]=ph.pnt[ph.fc[i].pnt[j]];
		}
		putFlatPolygon(pntbuff,ph.fc[i].NumberofVertices,ph.fc[i].NormalVector);
	}
}

void putFlatPolyhedron(smoothpolyhedron_t &ph)
{
	point3D_t pntbuff[32];
	int i,j;
	for (i=0;i<ph.NumberofFaces;i++) {
		for (j=0;j<ph.fc[i].NumberofVertices;j++) {
			pntbuff[j]=ph.pnt[ph.fc[i].pnt[j]];
		}
		putFlatPolygon(pntbuff,ph.fc[i].NumberofVertices,ph.fc[i].NormalVector);
	}
}

void putSmoothPolyhedron(smoothpolyhedron_t &ph)
{
	point3D_t pntbuff[32];
	vector3D_t vecbuff[32];
	int i,j;
	for (i=0;i<ph.NumberofFaces;i++) {
		for (j=0;j<ph.fc[i].NumberofVertices;j++) {
			pntbuff[j]=ph.pnt[ph.fc[i].pnt[j]];
			vecbuff[j]=ph.NormalVector[ph.fc[i].pnt[j]];
		}
		putSmoothPolygon(pntbuff,vecbuff,ph.fc[i].NumberofVertices);
	}
}

//////////// End of OpenGL drawShape Functions ver 3 ////////////

//////////////////////////////////////////////////////////////////
static void drawcharX(float x,float y,float z)
{
	drawLine(x,y,z,x+10,y+12,z);drawLine(x,y+12,z,x+10,y,z);
}

static void drawcharY(float x,float y,float z)
{
	drawLine(x+5,y,z,x+5,y+7,z);drawLine(x,y+12,z,x+5,y+7,z);drawLine(x+10,y+12,z,x+5,y+7,z);
}

static void drawcharZ(float x,float y,float z)
{
	drawLine(x,y+12,z,x+10,y+12,z);drawLine(x+10,y+12,z,x,y,z);drawLine(x,y,z,x+10,y,z);
}

void drawAxes(void)
{
#define HALFAXIS  220
#define HALFAXIS1 (HALFAXIS-10)
	point3D_t axes[14]={
		{-HALFAXIS,0,0},{HALFAXIS,0,0},{HALFAXIS1,5,0},{HALFAXIS1,0,0},{0,0,0},
		{0,-HALFAXIS,0},{0,HALFAXIS,0},{0,HALFAXIS1,5},{0,HALFAXIS1,0},{0,0,0},
		{0,0,-HALFAXIS},{0,0,HALFAXIS},{5,0,HALFAXIS1},{0,0,HALFAXIS1}
	};
	drawPolyline(axes,14);
	drawcharX(axes[1].x,axes[1].y,axes[1].z);
	drawcharY(axes[6].x,axes[6].y,axes[6].z);
	drawcharZ(axes[11].x-14,axes[11].y,axes[11].z);
}

void myInitView(int enableLighting)
{
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	GLfloat White[] = { 1.0, 1.0, 1.0, 1.0 }; // R,G,B,A
	GLfloat light0pos[] = { 500.0, 500.0, 1500.0, 0.0 }; // x,y,z,d
	glLightfv(GL_LIGHT0, GL_AMBIENT, White);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, White);
	glLightfv(GL_LIGHT0, GL_SPECULAR, White);

	//To set perspective view
	glMatrixMode(GL_PROJECTION);
  	glLoadIdentity();
	gluPerspective(40.0, 640. / 480., 100.0, 1300.0);
	glMatrixMode(GL_MODELVIEW);
	glEnable(GL_NORMALIZE); // normarize normal vectors
	glLoadIdentity();
	glLightfv(GL_LIGHT0, GL_POSITION, light0pos); //definition of light position must be here

	float leye=700.0f; float dip=0.65f; float rot=0.5f; //dip,rot[rad]
	float ly,lxz,lx,lz;
	ly=leye*sin(dip); lxz=leye*cos(dip);
	lx=lxz*sin(rot); lz=lxz*cos(rot);
	gluLookAt(lx,ly,lz, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);

	glMaterialfv(GL_FRONT, GL_AMBIENT, White); // Attribute of the Axes
	drawAxes();
	if (!enableLighting) {
		glDisable(GL_LIGHTING);
	}
}

