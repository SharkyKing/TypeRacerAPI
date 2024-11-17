﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TypeRacerAPI.Data;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GameLevelId")
                        .HasColumnType("int");

                    b.Property<int>("GameTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOver")
                        .HasColumnType("bit");

                    b.Property<long>("StartTime")
                        .HasColumnType("bigint");

                    b.Property<string>("Words")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameLevelId");

                    b.HasIndex("GameTypeId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameLevelClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GameLevelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GameLevel");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GameLevelName = "Beginner"
                        },
                        new
                        {
                            Id = 2,
                            GameLevelName = "Normal"
                        },
                        new
                        {
                            Id = 3,
                            GameLevelName = "Advanced"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameLogClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("LogTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("LogTypeId");

                    b.HasIndex("PlayerId");

                    b.ToTable("GameLog");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameTypeClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GameTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GameType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GameTypeName = "TimeAttack"
                        },
                        new
                        {
                            Id = 2,
                            GameTypeName = "FluentType"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.LogTypeClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("LogTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LogType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LogTypeName = "Error"
                        },
                        new
                        {
                            Id = 2,
                            LogTypeName = "GameLog"
                        },
                        new
                        {
                            Id = 3,
                            LogTypeName = "Information"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerClass", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("ConnectionGUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentWordIndex")
                        .HasColumnType("int");

                    b.Property<bool>("Finished")
                        .HasColumnType("bit");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<bool>("InputEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInitialized")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPartyLeader")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSpectator")
                        .HasColumnType("bit");

                    b.Property<int>("MistakeCount")
                        .HasColumnType("int");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WPM")
                        .HasColumnType("int");

                    b.Property<bool>("WordVisible")
                        .HasColumnType("bit");

                    b.Property<int?>("WordsStyleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("WordsStyleId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerGameResultTypeClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GifUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PlayerGameResultType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GifUrl = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExOGkweWlweTBuanJjeWN0d2xna3R2YzJ0YWVoZTRkNmZhMTV5MjZrayZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/xT0GqssRweIhlz209i/giphy.gif",
                            Text = "Congratulations!",
                            Title = "You WON!"
                        },
                        new
                        {
                            Id = 2,
                            GifUrl = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExNnBpM3dvYjgyYWdhaXJ0dzk3M2NkY3U3NzVzdzExamd6N2VkYTYweiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/B4uP3h97Hi2UaqS0E3/giphy.gif",
                            Text = "Better luck next time",
                            Title = "You lost :("
                        },
                        new
                        {
                            Id = 3,
                            GifUrl = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExNnBpM3dvYjgyYWdhaXJ0dzk3M2NkY3U3NzVzdzExamd6N2VkYTYweiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/B4uP3h97Hi2UaqS0E3/giphy.gif",
                            Text = "Be faster next time!",
                            Title = "Nobody won this game"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CooldownTime")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsTimedPower")
                        .HasColumnType("bit");

                    b.Property<string>("PlayerPowerKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerPowerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PlayerPower");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CooldownTime = 10,
                            ImagePath = "/images/freeze.png",
                            IsTimedPower = true,
                            PlayerPowerKey = "F",
                            PlayerPowerName = "Freeze"
                        },
                        new
                        {
                            Id = 2,
                            CooldownTime = 5,
                            ImagePath = "/images/rewind.png",
                            IsTimedPower = false,
                            PlayerPowerKey = "R",
                            PlayerPowerName = "Rewind"
                        },
                        new
                        {
                            Id = 3,
                            CooldownTime = 15,
                            ImagePath = "/images/invisible.png",
                            IsTimedPower = true,
                            PlayerPowerKey = "I",
                            PlayerPowerName = "Invisible"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerUseClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsOnCooldown")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReceived")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerPowerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerPowerId");

                    b.ToTable("PlayerPowerUses");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.WordsClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GameLevelId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameLevelId");

                    b.ToTable("Words");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GameLevelId = 1,
                            Text = "She goes to the zoo. She sees a lion. The lion roars. She sees an elephant. The elephant has a long trunk. She sees a turtle. The turtle is slow. She sees a rabbit. The rabbit has soft fur. She sees a gorilla. The gorilla is eating a banana."
                        },
                        new
                        {
                            Id = 2,
                            GameLevelId = 1,
                            Text = "She sits in the car. Her dad turns on the radio. A song plays. She taps her feet. She sways her head. Her dad laughs at her. He likes the song too. The song is over. The radio plays a different song. She does not like the new song. She sits quietly."
                        },
                        new
                        {
                            Id = 3,
                            GameLevelId = 1,
                            Text = "Today is Father's Day. Daniel surprises his father. He serves him breakfast. There are eggs, bacon, and orange juice on the tray. Daniel's father is happy. Later, they go to play tennis. Daniel stands on one side. He swings the ball. Daniel's father hits the ball back. Finally, they watch the sunset. What a great day!"
                        },
                        new
                        {
                            Id = 4,
                            GameLevelId = 1,
                            Text = "She eats a slice of cake. She drops a crumb. The ants can smell it. They crawl towards the crumb. She notices the ants. She does not want to kill them. She gets a cup. She puts the ants inside. She opens the window. She lets the ants go."
                        },
                        new
                        {
                            Id = 5,
                            GameLevelId = 1,
                            Text = "He goes to the petting zoo. There are many different animals. He pets the turtles. The turtles feel rough. He pets the sheep. The sheep feel wooly. He pets the cows. The cows feel smooth. He pets the bunnies. The bunnies feel fluffy. He tells his mom he wants a pet. His mom says he can get one tomorrow."
                        },
                        new
                        {
                            Id = 6,
                            GameLevelId = 2,
                            Text = "In the resplendent glow of the cerulean twilight, she contemplated the ephemeral nature of existence. The ineffable beauty of the cosmos seemed to whisper secrets of the ancients, urging her to embrace the serendipity that life bestowed. Each star, a scintilla of hope, beckoned her toward the infinitesimal mysteries that lay beyond the horizon of her understanding."
                        },
                        new
                        {
                            Id = 7,
                            GameLevelId = 2,
                            Text = "The dichotomy between epistemology and ontology presents a formidable challenge for those endeavoring to delineate the parameters of human knowledge. As we navigate the labyrinthine corridors of perception and reality, it becomes increasingly imperative to scrutinize the veracity of our beliefs. Only through rigorous dialectics can one aspire to transcend the ephemeral nature of subjective experience and approach an authentic comprehension of existence."
                        },
                        new
                        {
                            Id = 8,
                            GameLevelId = 2,
                            Text = "In the realm of astrophysics, the phenomenon of black holes epitomizes the quintessence of the universe’s enigmatic nature. These celestial behemoths possess gravitational fields so potent that not even light can elude their grasp, rendering them imperceptible to direct observation. The implications of their existence challenge our fundamental understanding of time and space, necessitating a paradigm shift in our approach to cosmological studies."
                        },
                        new
                        {
                            Id = 9,
                            GameLevelId = 2,
                            Text = "The ramifications of the Industrial Revolution were far-reaching, catalyzing a profound metamorphosis in societal structures. Urbanization burgeoned as agrarian communities dissipated, leading to an unprecedented influx of individuals into burgeoning metropolises. This epoch was characterized by both technological advancements and the concomitant emergence of socio-economic disparities, fostering a milieu of discontent that would later precipitate calls for reform and revolution."
                        },
                        new
                        {
                            Id = 10,
                            GameLevelId = 2,
                            Text = "The painter's oeuvre is a veritable tapestry of emotive expression, interweaving vibrant hues with intricate brushwork. Each canvas serves as a conduit for the artist's innermost contemplations, reflecting a juxtaposition of the sublime and the grotesque. Through a meticulous examination of form and color, one can discern the underlying ethos that drives the narrative of human experience, compelling viewers to confront their own existential dilemmas."
                        },
                        new
                        {
                            Id = 11,
                            GameLevelId = 3,
                            Text = "In Lithuanian culture, tradicijos play a vital role in the community. People often gather to celebrate events like Jono Naktis, which showcases their respect for the seasons and nature. This celebration is a perfect example of how traditions unite people and strengthen their identity."
                        },
                        new
                        {
                            Id = 12,
                            GameLevelId = 3,
                            Text = "When contemplating existence, many ask, \"Koks yra tikrasis zmogaus prigimtis?\" This question delves into the essence of humanity, exploring whether we are mere physical beings or if we possess a deeper, spiritual side. In Lithuania, philosophers discuss the intersection of morality and ethics, which remains relevant globally."
                        },
                        new
                        {
                            Id = 13,
                            GameLevelId = 3,
                            Text = "Climate change is a pressing global issue, and Lithuania is no exception. Scientists are researching how this phenomenon affects agriculture and biodiversity. They found that ekstremalios oro salygos pose a threat to food security and the economy, emphasizing the need for urgent action."
                        },
                        new
                        {
                            Id = 14,
                            GameLevelId = 3,
                            Text = "Lithuanian authors often reflect social and political changes in their works. By employing metaphors and simboliai, they convey complex ideas about identity and freedom. Critics argue that such literature not only enriches the cultural landscape but also stimulates discussions about what true freedom means and how it can be achieved."
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.WordsStyleClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StyleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fontFamily")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fontStyle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fontWeight")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WordsStyle");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StyleName = "BoldDecorator",
                            fontFamily = "Arial, sans-serif",
                            fontWeight = "bold"
                        },
                        new
                        {
                            Id = 2,
                            StyleName = "ItalicDecorator",
                            fontFamily = "Georgia, serif"
                        },
                        new
                        {
                            Id = 3,
                            StyleName = "FancyFontDecorator",
                            fontFamily = "Courier New, monospace",
                            fontStyle = "normal",
                            fontWeight = "normal"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameClass", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.GameLevelClass", "GameLevel")
                        .WithMany("Games")
                        .HasForeignKey("GameLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TypeRacerAPI.BaseClasses.GameTypeClass", "GameType")
                        .WithMany("Games")
                        .HasForeignKey("GameTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameLevel");

                    b.Navigation("GameType");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameLogClass", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.GameClass", "Game")
                        .WithMany("GameLog")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("TypeRacerAPI.BaseClasses.LogTypeClass", "LogType")
                        .WithMany("GameLog")
                        .HasForeignKey("LogTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TypeRacerAPI.BaseClasses.PlayerClass", "Player")
                        .WithMany("GameLog")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Game");

                    b.Navigation("LogType");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerClass", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.GameClass", "Game")
                        .WithMany("Players")
                        .HasForeignKey("GameId");

                    b.HasOne("TypeRacerAPI.BaseClasses.WordsStyleClass", "WordsStyle")
                        .WithMany("Players")
                        .HasForeignKey("WordsStyleId");

                    b.Navigation("Game");

                    b.Navigation("WordsStyle");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerUseClass", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.PlayerClass", "Player")
                        .WithMany("PlayerPowerUses")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TypeRacerAPI.BaseClasses.PlayerPowerClass", "PlayerPower")
                        .WithMany("PlayerPowerUses")
                        .HasForeignKey("PlayerPowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("PlayerPower");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.WordsClass", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.GameLevelClass", "GameLevel")
                        .WithMany("Words")
                        .HasForeignKey("GameLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameLevel");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameClass", b =>
                {
                    b.Navigation("GameLog");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameLevelClass", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Words");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameTypeClass", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.LogTypeClass", b =>
                {
                    b.Navigation("GameLog");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerClass", b =>
                {
                    b.Navigation("GameLog");

                    b.Navigation("PlayerPowerUses");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerClass", b =>
                {
                    b.Navigation("PlayerPowerUses");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.WordsStyleClass", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
