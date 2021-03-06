﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Pb.Wechat.EntityFrameworkCore;
using System;

namespace Pb.Wechat.Migrations.GroupMessageDb
{
    [DbContext(typeof(GroupMessageDbContext))]
    [Migration("20180310092155_add_groupmessages")]
    partial class add_groupmessages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Pb.Wechat.TaskGroupMessages.TaskGroupMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArticleGroupID");

                    b.Property<string>("ArticleGroupMediaID")
                        .HasMaxLength(200);

                    b.Property<string>("ArticleGroupName")
                        .HasMaxLength(500);

                    b.Property<int?>("ArticleID");

                    b.Property<string>("ArticleMediaID")
                        .HasMaxLength(200);

                    b.Property<string>("ArticleName")
                        .HasMaxLength(500);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId");

                    b.Property<int>("GroupID");

                    b.Property<int?>("ImageID");

                    b.Property<string>("ImageMediaID")
                        .HasMaxLength(200);

                    b.Property<string>("ImageName")
                        .HasMaxLength(500);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId");

                    b.Property<int>("MessageID");

                    b.Property<string>("MessageType");

                    b.Property<int>("MpID");

                    b.Property<int?>("VideoID");

                    b.Property<string>("VideoMediaID")
                        .HasMaxLength(200);

                    b.Property<string>("VideoName")
                        .HasMaxLength(500);

                    b.Property<int?>("VoiceID");

                    b.Property<string>("VoiceMediaID")
                        .HasMaxLength(200);

                    b.Property<string>("VoiceName")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("TaskGroupMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
