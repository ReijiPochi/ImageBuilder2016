#define MAXSIZE 8192

typedef struct {
	float x;
	float y;
	float z;
} point3D_t;

typedef struct {
	float r;
	float g;
	float b;
} color_t;

typedef struct {
	float v[4];
} vector3D_t;

typedef struct {
	float m[4][4];
} matrix3D_t;

typedef struct {
	int NumberofVertices; //in the face
	short int pnt[8];
	vector3D_t NormalVector; //of the face
} face_t;

typedef struct {
	int NumberofVertices; //of the object
	point3D_t pnt[MAXSIZE];
	vector3D_t NormalVector[MAXSIZE]; // at the pnt
	int NumberofFaces; //of the object
	face_t fc[MAXSIZE];
} smoothpolyhedron_t;

typedef struct {
	int NumberofVertices; //of the object
	point3D_t pnt[MAXSIZE];
	int NumberofFaces; //of the object
	face_t fc[MAXSIZE];
} polyhedron_t;

matrix3D_t createIdentity(void);
matrix3D_t operator * (matrix3D_t a,matrix3D_t b);
vector3D_t operator * (matrix3D_t a, vector3D_t b);
matrix3D_t translationMTX(float dx,float dy,float dz);
matrix3D_t rotationXMTX(float theta);
matrix3D_t rotationYMTX(float theta);
matrix3D_t rotationZMTX(float theta);
matrix3D_t scalingMTX(float factorx,float factory,float factorz);
vector3D_t Point2Vector(point3D_t pnt);
point3D_t Vector2Point(vector3D_t vec);
vector3D_t unitVector(vector3D_t vec);
float operator * (vector3D_t a, vector3D_t b);
vector3D_t operator ^ (vector3D_t a, vector3D_t b);
vector3D_t operator - (vector3D_t v1,vector3D_t v0);
vector3D_t operator - (vector3D_t v);
vector3D_t operator * (float r, vector3D_t b);
vector3D_t operator * (vector3D_t b, float r);

void setColor(float red,float green,float blue);
void setColor(color_t col);
void drawDot(float x,float y, float z);
void drawLine(float x1, float y1, float z1, float x2, float y2, float z2);
void drawLine(point3D_t p1,point3D_t p2);
void drawPolyline(point3D_t pnt[],int n);
void drawPolygon(point3D_t pnt[],int n);
void fillPolygon(point3D_t pnt[],int n,color_t color);
void gradatePolygon(point3D_t pnt[],color_t col[],int num);
void putFlatPolygon(point3D_t pnt[],int num,vector3D_t NVector);
void putSmoothPolygon(point3D_t pnt[],vector3D_t NVector[],int num);
void putFlatPolyhedron(polyhedron_t &ph);
void putFlatPolyhedron(smoothpolyhedron_t &ph);
void putSmoothPolyhedron(smoothpolyhedron_t &ph);

void drawAxes(void);
void myInitView(int enableLighting);

