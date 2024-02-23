--
-- PostgreSQL database dump
--

-- Dumped from database version 14.2 (Debian 14.2-1.pgdg110+1)
-- Dumped by pg_dump version 15.3

-- Started on 2024-01-14 14:57:31

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--


ALTER SCHEMA public OWNER TO postgres;

--
-- TOC entry 3341 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 209 (class 1259 OID 16385)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 210 (class 1259 OID 16390)
-- Name: roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.roles (
    id text NOT NULL,
    name character varying(256),
    normalized_name character varying(256),
    concurrency_stamp text
);


ALTER TABLE public.roles OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 16404)
-- Name: user_roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_roles (
    user_id text NOT NULL,
    role_id text NOT NULL
);


ALTER TABLE public.user_roles OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 16397)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id text NOT NULL,
    created_date_time timestamp with time zone NOT NULL,
    inn text,
    org_name text,
    user_name character varying(256),
    normalized_user_name character varying(256),
    email character varying(256),
    normalized_email character varying(256),
    email_confirmed boolean NOT NULL,
    password_hash text,
    security_stamp text,
    concurrency_stamp text,
    phone_number text,
    phone_number_confirmed boolean NOT NULL,
    two_factor_enabled boolean NOT NULL,
    lockout_end timestamp with time zone,
    lockout_enabled boolean NOT NULL,
    access_failed_count integer NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 3332 (class 0 OID 16385)
-- Dependencies: 209
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20240109124930_AddAuthorization	7.0.9
\.


--
-- TOC entry 3333 (class 0 OID 16390)
-- Dependencies: 210
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.roles (id, name, normalized_name, concurrency_stamp) FROM stdin;
d5381bc9-445e-4b49-8692-f0baced84d4f	Admin	ADMIN	14.01.2024 13:01:58
0f5d14e7-b135-4976-961a-733881478e9c	Uploader	UPLOADER	14.01.2024 13:01:58
a0b53cc2-9d0b-433d-8b4f-47eebb30fc59	User	USER	14.01.2024 13:01:58
\.


--
-- TOC entry 3335 (class 0 OID 16404)
-- Dependencies: 212
-- Data for Name: user_roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_roles (user_id, role_id) FROM stdin;
430dd867-c656-4faa-8c03-cbadeda37b2e	d5381bc9-445e-4b49-8692-f0baced84d4f
407f2b9a-a887-40f1-a640-a3fc5ed523e7	d5381bc9-445e-4b49-8692-f0baced84d4f
\.


--
-- TOC entry 3334 (class 0 OID 16397)
-- Dependencies: 211
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, created_date_time, inn, org_name, user_name, normalized_user_name, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number, phone_number_confirmed, two_factor_enabled, lockout_end, lockout_enabled, access_failed_count) FROM stdin;
430dd867-c656-4faa-8c03-cbadeda37b2e	2024-01-14 10:02:35.295541+00	\N	\N	verba	VERBA	verba	VERBA	f	AQAAAAEAACcQAAAAECczAXnC+KzZhS2j4JrutO8YJr4t4qkowBhyJcyRNzIcEkOvKF38gR+YrOVr8+Xqag==	4M23FBL46ZIC5T6IETHJ4RBG632TSCI2	ac4ba53f-e6fd-41e4-a32c-98f617331cee	3-42-66	f	f	\N	t	0
407f2b9a-a887-40f1-a640-a3fc5ed523e7	2024-01-14 10:40:51.611617+00	\N	\N	admin	ADMIN	admin	ADMIN	f	AQAAAAEAACcQAAAAEDhOkYJCW08yX0SoRSxdX5E1TRvqETCsF0vMjEl0v1pWsuhiiYqtOZskvJp2lzb5ZQ==	LW6RQ6ZM3IZWIQ2U4RNPOW4EF64ITS4M	51fb996a-7a6e-438b-b5e8-8c1bc45d3b8b	3-42-66	f	f	\N	t	0
\.


--
-- TOC entry 3179 (class 2606 OID 16389)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3181 (class 2606 OID 16396)
-- Name: roles PK_roles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT "PK_roles" PRIMARY KEY (id);


--
-- TOC entry 3190 (class 2606 OID 16410)
-- Name: user_roles PK_user_roles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT "PK_user_roles" PRIMARY KEY (role_id, user_id);


--
-- TOC entry 3185 (class 2606 OID 16403)
-- Name: users PK_users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT "PK_users" PRIMARY KEY (id);


--
-- TOC entry 3183 (class 1259 OID 16424)
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "EmailIndex" ON public.users USING btree (normalized_email);


--
-- TOC entry 3187 (class 1259 OID 16422)
-- Name: IX_user_roles_role_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_user_roles_role_id" ON public.user_roles USING btree (role_id);


--
-- TOC entry 3188 (class 1259 OID 16423)
-- Name: IX_user_roles_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_user_roles_user_id" ON public.user_roles USING btree (user_id);


--
-- TOC entry 3182 (class 1259 OID 16421)
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public.roles USING btree (normalized_name) WHERE (normalized_name IS NOT NULL);


--
-- TOC entry 3186 (class 1259 OID 16425)
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "UserNameIndex" ON public.users USING btree (normalized_user_name) WHERE (normalized_user_name IS NOT NULL);


--
-- TOC entry 3191 (class 2606 OID 16411)
-- Name: user_roles FK_user_roles_roles_role_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT "FK_user_roles_roles_role_id" FOREIGN KEY (role_id) REFERENCES public.roles(id) ON DELETE CASCADE;


--
-- TOC entry 3192 (class 2606 OID 16416)
-- Name: user_roles FK_user_roles_users_user_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT "FK_user_roles_users_user_id" FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;


--
-- TOC entry 3342 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2024-01-14 14:57:31

--
-- PostgreSQL database dump complete
--

