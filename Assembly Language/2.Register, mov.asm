%include "io64.inc"

section .text
global main
main:
    ; 8bit = 1byte
    ; 16bit = 2byte = 1word
    ; 32bit = 4byte = 2word = 1dword (double-word)
    ; 64bit = 8byte = 4word = 1qword (quad-word)
    
    ; 레지스터 종류는 여러가지지만, 일반적으로 쓰이는 건 범용 레지스터(General Purpose Register)
    ; 32비트에서는 EAX, EBX, ECX, EDX가 있고, 64비트에서는 동일한 기능의 RAX, RBX, RCX, RDX로 확
    ; eax: Extended Accumulator Register, 사칙/논리연산에 일반적으로 사용되는 저장 공간
    ; ebx: 메모리 주소 저장
    ; ecx: 반복 구문(for/while)에서 반복횟수 저장
    ; edx: eax와 함께 사용되며 데이터 저장용. 부호 확장/큰 수 연산에서 사용
    
    ; Extended 에서 유추할 수 있듯이 AX만 쓰면 16비트의 Accumulator Register를 의미 (8비트의 ah, al로 구성)
    ; 같은 원리로 ECX에서 마지막 2바이트는 CX로, 1바이트의 ch, cl로 구성된다.
    
    ; 어셈블리 명령어
    ; mov: 값 이동(무브) 명령어
    ;  [사용법1] mov {r1} {const} : 레지스터(r1)에 상수(const) 값을 넣는다
    ;  [사용법2] mov {r1} {r2} : 레지스터(r1)에 다른 레지스터(r2)의 값을 넣는다    
    mov eax, 0x1234 ; 0x는 16진수, 0b는 2진수
    mov rbx, 0x12345678
    mov cl, 0xff ; CX의 최하위 1바이트인데, 1바이트를 초과하는 0xffff 같은 값을 넣으면 오버플로우 에러가 난다.
    
    mov al, 0x00 ; eax에 1234가 저장되어 있는데, 하위 1바이트(al)을 00으로 하면 0x1200이 된다.
    mov rax, rdx
    
    ; 참고로 디버그 모드(F5)로 실행하면 코드 첫 부분에 mov rbp, rsp 가 자동으로 붙는다.
    ; 정확한 디버깅을 위해 베이스 포인터 레지스터(RBP)에 스택 포인터 레지스터(RSP)를 넣는 부분.
    
    xor rax, rax
    ret
    
;section .data