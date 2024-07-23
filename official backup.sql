PGDMP  $                    |           HR    16.3    16.3 C    D           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            E           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            F           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            G           1262    16398    HR    DATABASE     |   CREATE DATABASE "HR" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Bulgarian_Bulgaria.1251';
    DROP DATABASE "HR";
                postgres    false            �            1259    16541    bonuses    TABLE     Y   CREATE TABLE public.bonuses (
    emp_id integer NOT NULL,
    bonus double precision
);
    DROP TABLE public.bonuses;
       public         heap    postgres    false            �            1259    16526 
   developers    TABLE     `   CREATE TABLE public.developers (
    dev_id integer NOT NULL,
    dev_skill integer NOT NULL
);
    DROP TABLE public.developers;
       public         heap    postgres    false            �            1259    16520    developerskills    TABLE     h   CREATE TABLE public.developerskills (
    id_skill integer NOT NULL,
    skill character varying(30)
);
 #   DROP TABLE public.developerskills;
       public         heap    postgres    false            �            1259    16519    developerskills_id_skill_seq    SEQUENCE     �   CREATE SEQUENCE public.developerskills_id_skill_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 3   DROP SEQUENCE public.developerskills_id_skill_seq;
       public          postgres    false    226            H           0    0    developerskills_id_skill_seq    SEQUENCE OWNED BY     ]   ALTER SEQUENCE public.developerskills_id_skill_seq OWNED BY public.developerskills.id_skill;
          public          postgres    false    225            �            1259    16639    developertitles    TABLE     G   CREATE TABLE public.developertitles (
    title_id integer NOT NULL
);
 #   DROP TABLE public.developertitles;
       public         heap    postgres    false            �            1259    16400 	   employees    TABLE     �   CREATE TABLE public.employees (
    emp_id integer NOT NULL,
    firstname character varying(100),
    lastname character varying(100),
    salary numeric(10,2),
    yearofbirth integer
);
    DROP TABLE public.employees;
       public         heap    postgres    false            �            1259    16399    employees_emp_id_seq    SEQUENCE     �   CREATE SEQUENCE public.employees_emp_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public.employees_emp_id_seq;
       public          postgres    false    218            I           0    0    employees_emp_id_seq    SEQUENCE OWNED BY     M   ALTER SEQUENCE public.employees_emp_id_seq OWNED BY public.employees.emp_id;
          public          postgres    false    217            �            1259    16551    employeetitles    TABLE     [   CREATE TABLE public.employeetitles (
    emp_id integer NOT NULL,
    emp_title integer
);
 "   DROP TABLE public.employeetitles;
       public         heap    postgres    false            �            1259    16425 	   languages    TABLE     h   CREATE TABLE public.languages (
    language_id integer NOT NULL,
    language character varying(30)
);
    DROP TABLE public.languages;
       public         heap    postgres    false            �            1259    16424    languages_language_id_seq    SEQUENCE     �   CREATE SEQUENCE public.languages_language_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 0   DROP SEQUENCE public.languages_language_id_seq;
       public          postgres    false    220            J           0    0    languages_language_id_seq    SEQUENCE OWNED BY     W   ALTER SEQUENCE public.languages_language_id_seq OWNED BY public.languages.language_id;
          public          postgres    false    219            �            1259    16472    languagespoken    TABLE     j   CREATE TABLE public.languagespoken (
    emp_id integer NOT NULL,
    language_spoken integer NOT NULL
);
 "   DROP TABLE public.languagespoken;
       public         heap    postgres    false            �            1259    16504    manager    TABLE     d   CREATE TABLE public.manager (
    idmanager integer NOT NULL,
    idsubordinate integer NOT NULL
);
    DROP TABLE public.manager;
       public         heap    postgres    false            �            1259    16595    managertitlles    TABLE     F   CREATE TABLE public.managertitlles (
    title_id integer NOT NULL
);
 "   DROP TABLE public.managertitlles;
       public         heap    postgres    false            �            1259    16594    managertitlles_title_id_seq    SEQUENCE     �   CREATE SEQUENCE public.managertitlles_title_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public.managertitlles_title_id_seq;
       public          postgres    false    231            K           0    0    managertitlles_title_id_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public.managertitlles_title_id_seq OWNED BY public.managertitlles.title_id;
          public          postgres    false    230            �            1259    16488    titles    TABLE     `   CREATE TABLE public.titles (
    title_id integer NOT NULL,
    title character varying(100)
);
    DROP TABLE public.titles;
       public         heap    postgres    false            �            1259    16487    titles_title_id_seq    SEQUENCE     �   CREATE SEQUENCE public.titles_title_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.titles_title_id_seq;
       public          postgres    false    223            L           0    0    titles_title_id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.titles_title_id_seq OWNED BY public.titles.title_id;
          public          postgres    false    222            �           2604    16523    developerskills id_skill    DEFAULT     �   ALTER TABLE ONLY public.developerskills ALTER COLUMN id_skill SET DEFAULT nextval('public.developerskills_id_skill_seq'::regclass);
 G   ALTER TABLE public.developerskills ALTER COLUMN id_skill DROP DEFAULT;
       public          postgres    false    225    226    226            ~           2604    16403    employees emp_id    DEFAULT     t   ALTER TABLE ONLY public.employees ALTER COLUMN emp_id SET DEFAULT nextval('public.employees_emp_id_seq'::regclass);
 ?   ALTER TABLE public.employees ALTER COLUMN emp_id DROP DEFAULT;
       public          postgres    false    218    217    218                       2604    16428    languages language_id    DEFAULT     ~   ALTER TABLE ONLY public.languages ALTER COLUMN language_id SET DEFAULT nextval('public.languages_language_id_seq'::regclass);
 D   ALTER TABLE public.languages ALTER COLUMN language_id DROP DEFAULT;
       public          postgres    false    219    220    220            �           2604    16598    managertitlles title_id    DEFAULT     �   ALTER TABLE ONLY public.managertitlles ALTER COLUMN title_id SET DEFAULT nextval('public.managertitlles_title_id_seq'::regclass);
 F   ALTER TABLE public.managertitlles ALTER COLUMN title_id DROP DEFAULT;
       public          postgres    false    231    230    231            �           2604    16491    titles title_id    DEFAULT     r   ALTER TABLE ONLY public.titles ALTER COLUMN title_id SET DEFAULT nextval('public.titles_title_id_seq'::regclass);
 >   ALTER TABLE public.titles ALTER COLUMN title_id DROP DEFAULT;
       public          postgres    false    222    223    223            =          0    16541    bonuses 
   TABLE DATA           0   COPY public.bonuses (emp_id, bonus) FROM stdin;
    public          postgres    false    228   �L       <          0    16526 
   developers 
   TABLE DATA           7   COPY public.developers (dev_id, dev_skill) FROM stdin;
    public          postgres    false    227   0M       ;          0    16520    developerskills 
   TABLE DATA           :   COPY public.developerskills (id_skill, skill) FROM stdin;
    public          postgres    false    226   `M       A          0    16639    developertitles 
   TABLE DATA           3   COPY public.developertitles (title_id) FROM stdin;
    public          postgres    false    232   -N       3          0    16400 	   employees 
   TABLE DATA           U   COPY public.employees (emp_id, firstname, lastname, salary, yearofbirth) FROM stdin;
    public          postgres    false    218   NN       >          0    16551    employeetitles 
   TABLE DATA           ;   COPY public.employeetitles (emp_id, emp_title) FROM stdin;
    public          postgres    false    229   O       5          0    16425 	   languages 
   TABLE DATA           :   COPY public.languages (language_id, language) FROM stdin;
    public          postgres    false    220   KO       6          0    16472    languagespoken 
   TABLE DATA           A   COPY public.languagespoken (emp_id, language_spoken) FROM stdin;
    public          postgres    false    221   P       9          0    16504    manager 
   TABLE DATA           ;   COPY public.manager (idmanager, idsubordinate) FROM stdin;
    public          postgres    false    224   cP       @          0    16595    managertitlles 
   TABLE DATA           2   COPY public.managertitlles (title_id) FROM stdin;
    public          postgres    false    231   �P       8          0    16488    titles 
   TABLE DATA           1   COPY public.titles (title_id, title) FROM stdin;
    public          postgres    false    223   �P       M           0    0    developerskills_id_skill_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.developerskills_id_skill_seq', 1, true);
          public          postgres    false    225            N           0    0    employees_emp_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public.employees_emp_id_seq', 4, true);
          public          postgres    false    217            O           0    0    languages_language_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public.languages_language_id_seq', 7, true);
          public          postgres    false    219            P           0    0    managertitlles_title_id_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.managertitlles_title_id_seq', 1, false);
          public          postgres    false    230            Q           0    0    titles_title_id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public.titles_title_id_seq', 6, true);
          public          postgres    false    222            �           2606    16545    bonuses bonuses_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.bonuses
    ADD CONSTRAINT bonuses_pkey PRIMARY KEY (emp_id);
 >   ALTER TABLE ONLY public.bonuses DROP CONSTRAINT bonuses_pkey;
       public            postgres    false    228            �           2606    16530    developers developers_pkey 
   CONSTRAINT     g   ALTER TABLE ONLY public.developers
    ADD CONSTRAINT developers_pkey PRIMARY KEY (dev_id, dev_skill);
 D   ALTER TABLE ONLY public.developers DROP CONSTRAINT developers_pkey;
       public            postgres    false    227    227            �           2606    16525 $   developerskills developerskills_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.developerskills
    ADD CONSTRAINT developerskills_pkey PRIMARY KEY (id_skill);
 N   ALTER TABLE ONLY public.developerskills DROP CONSTRAINT developerskills_pkey;
       public            postgres    false    226            �           2606    16643 $   developertitles developertitles_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.developertitles
    ADD CONSTRAINT developertitles_pkey PRIMARY KEY (title_id);
 N   ALTER TABLE ONLY public.developertitles DROP CONSTRAINT developertitles_pkey;
       public            postgres    false    232            �           2606    16405    employees employees_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (emp_id);
 B   ALTER TABLE ONLY public.employees DROP CONSTRAINT employees_pkey;
       public            postgres    false    218            �           2606    16555 "   employeetitles employeetitles_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public.employeetitles
    ADD CONSTRAINT employeetitles_pkey PRIMARY KEY (emp_id);
 L   ALTER TABLE ONLY public.employeetitles DROP CONSTRAINT employeetitles_pkey;
       public            postgres    false    229            �           2606    16430    languages languages_pkey 
   CONSTRAINT     _   ALTER TABLE ONLY public.languages
    ADD CONSTRAINT languages_pkey PRIMARY KEY (language_id);
 B   ALTER TABLE ONLY public.languages DROP CONSTRAINT languages_pkey;
       public            postgres    false    220            �           2606    16476 "   languagespoken languagespoken_pkey 
   CONSTRAINT     u   ALTER TABLE ONLY public.languagespoken
    ADD CONSTRAINT languagespoken_pkey PRIMARY KEY (emp_id, language_spoken);
 L   ALTER TABLE ONLY public.languagespoken DROP CONSTRAINT languagespoken_pkey;
       public            postgres    false    221    221            �           2606    16508    manager manager_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.manager
    ADD CONSTRAINT manager_pkey PRIMARY KEY (idmanager, idsubordinate);
 >   ALTER TABLE ONLY public.manager DROP CONSTRAINT manager_pkey;
       public            postgres    false    224    224            �           2606    16600 "   managertitlles managertitlles_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.managertitlles
    ADD CONSTRAINT managertitlles_pkey PRIMARY KEY (title_id);
 L   ALTER TABLE ONLY public.managertitlles DROP CONSTRAINT managertitlles_pkey;
       public            postgres    false    231            �           2606    16493    titles titles_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.titles
    ADD CONSTRAINT titles_pkey PRIMARY KEY (title_id);
 <   ALTER TABLE ONLY public.titles DROP CONSTRAINT titles_pkey;
       public            postgres    false    223            �           2606    16546    bonuses bonuses_emp_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.bonuses
    ADD CONSTRAINT bonuses_emp_id_fkey FOREIGN KEY (emp_id) REFERENCES public.employees(emp_id);
 E   ALTER TABLE ONLY public.bonuses DROP CONSTRAINT bonuses_emp_id_fkey;
       public          postgres    false    218    4740    228            �           2606    16531 !   developers developers_dev_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.developers
    ADD CONSTRAINT developers_dev_id_fkey FOREIGN KEY (dev_id) REFERENCES public.employees(emp_id);
 K   ALTER TABLE ONLY public.developers DROP CONSTRAINT developers_dev_id_fkey;
       public          postgres    false    218    227    4740            �           2606    16536 $   developers developers_dev_skill_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.developers
    ADD CONSTRAINT developers_dev_skill_fkey FOREIGN KEY (dev_skill) REFERENCES public.developerskills(id_skill);
 N   ALTER TABLE ONLY public.developers DROP CONSTRAINT developers_dev_skill_fkey;
       public          postgres    false    227    226    4750            �           2606    16644 -   developertitles developertitles_title_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.developertitles
    ADD CONSTRAINT developertitles_title_id_fkey FOREIGN KEY (title_id) REFERENCES public.titles(title_id);
 W   ALTER TABLE ONLY public.developertitles DROP CONSTRAINT developertitles_title_id_fkey;
       public          postgres    false    232    223    4746            �           2606    16556 )   employeetitles employeetitles_emp_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.employeetitles
    ADD CONSTRAINT employeetitles_emp_id_fkey FOREIGN KEY (emp_id) REFERENCES public.employees(emp_id);
 S   ALTER TABLE ONLY public.employeetitles DROP CONSTRAINT employeetitles_emp_id_fkey;
       public          postgres    false    4740    218    229            �           2606    16561 ,   employeetitles employeetitles_emp_title_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.employeetitles
    ADD CONSTRAINT employeetitles_emp_title_fkey FOREIGN KEY (emp_title) REFERENCES public.titles(title_id);
 V   ALTER TABLE ONLY public.employeetitles DROP CONSTRAINT employeetitles_emp_title_fkey;
       public          postgres    false    4746    229    223            �           2606    16477 )   languagespoken languagespoken_emp_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.languagespoken
    ADD CONSTRAINT languagespoken_emp_id_fkey FOREIGN KEY (emp_id) REFERENCES public.employees(emp_id);
 S   ALTER TABLE ONLY public.languagespoken DROP CONSTRAINT languagespoken_emp_id_fkey;
       public          postgres    false    218    4740    221            �           2606    16482 2   languagespoken languagespoken_language_spoken_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.languagespoken
    ADD CONSTRAINT languagespoken_language_spoken_fkey FOREIGN KEY (language_spoken) REFERENCES public.languages(language_id);
 \   ALTER TABLE ONLY public.languagespoken DROP CONSTRAINT languagespoken_language_spoken_fkey;
       public          postgres    false    221    4742    220            �           2606    16509    manager manager_idmanager_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.manager
    ADD CONSTRAINT manager_idmanager_fkey FOREIGN KEY (idmanager) REFERENCES public.employees(emp_id);
 H   ALTER TABLE ONLY public.manager DROP CONSTRAINT manager_idmanager_fkey;
       public          postgres    false    4740    218    224            �           2606    16514 "   manager manager_idsubordinate_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.manager
    ADD CONSTRAINT manager_idsubordinate_fkey FOREIGN KEY (idsubordinate) REFERENCES public.employees(emp_id);
 L   ALTER TABLE ONLY public.manager DROP CONSTRAINT manager_idsubordinate_fkey;
       public          postgres    false    224    218    4740            =   4   x���  ���0^A�q�9D_I.����D7LI4�C"�^&ԋ�~��C      <       x�34�44�2��\���f@�Ȉ+F��� 6��      ;   �   x�M�=o�0��_���o�1IA��֨��,bj��	_��F,L�t��s
Ks6�
���u�0yNG(��_+��ud�Bk�1r��T���][辶(m�|���7�!�)
&D�`���J�·�S���k��{[R���*�)cd]g�;�L�g�)>w[��l_J4ܕ/����3bކ>���7����=�      A      x�3�2����� �%      3   �   x�]���0E׷CJ�/p��.�4vJA�F�����cu'9�3s�޲��%Qj�3�a$U�q~�!�ӭ{�XSӠ��e�ZnQ��+\�{�.R�����oΏ�l.��@N��i{�F�0�uqK�Ap�;�~a��^>>`��%w>&���/��׺8v��y���3��G�K?      >   3   x�ȱ  ��aB�a�9�K�*$5a�<X�;���o��(�A��b�      5   �   x�%���0���?���zT�~!���Th�
ŴT_�Rn�Y�L��j:iZZb���Z����blZ�����?�K��T���s�a����8���N� ŠG�X?a�Xc|b��SVĖXո>��U/g�q��=aY����g�����P=�r�?�JGw�;|,��9D?�      6   P   x�%���@߸��e�^�����T%68c�ch�i�	F�U��PX���x�i�A��J�=����$��>�^�n      9   0   x���  �7c��g1�_��c'YA��Vs����Ǵ�Â�7H~��      @      x�3�2������� 	ci      8   F   x�3�N���/RpI-K��/H-�2��M�KL�L8�s*�K��8��0-0e�.�E��%�E\1z\\\ ��S     