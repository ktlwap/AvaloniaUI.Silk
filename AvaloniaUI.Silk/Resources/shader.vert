#version 330 core
layout (location = 0) in vec3 vPos;

uniform mat4 uModel;
uniform mat4 uView;

void main()
{
    gl_Position = uView * uModel * vec4(vPos, 1.0);
}