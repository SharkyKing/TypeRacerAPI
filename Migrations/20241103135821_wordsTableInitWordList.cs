using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TypeRacerAPI.Migrations
{
    /// <inheritdoc />
    public partial class wordsTableInitWordList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "GameLevelId", "Words" },
                values: new object[,]
                {
                    { 1, 1, "She goes to the zoo. She sees a lion. The lion roars. She sees an elephant. The elephant has a long trunk. She sees a turtle. The turtle is slow. She sees a rabbit. The rabbit has soft fur. She sees a gorilla. The gorilla is eating a banana." },
                    { 2, 1, "She sits in the car. Her dad turns on the radio. A song plays. She taps her feet. She sways her head. Her dad laughs at her. He likes the song too. The song is over. The radio plays a different song. She does not like the new song. She sits quietly." },
                    { 3, 1, "Today is Father's Day. Daniel surprises his father. He serves him breakfast. There are eggs, bacon, and orange juice on the tray. Daniel's father is happy. Later, they go to play tennis. Daniel stands on one side. He swings the ball. Daniel's father hits the ball back. Finally, they watch the sunset. What a great day!" },
                    { 4, 1, "She eats a slice of cake. She drops a crumb. The ants can smell it. They crawl towards the crumb. She notices the ants. She does not want to kill them. She gets a cup. She puts the ants inside. She opens the window. She lets the ants go." },
                    { 5, 1, "He goes to the petting zoo. There are many different animals. He pets the turtles. The turtles feel rough. He pets the sheep. The sheep feel wooly. He pets the cows. The cows feel smooth. He pets the bunnies. The bunnies feel fluffy. He tells his mom he wants a pet. His mom says he can get one tomorrow." },
                    { 6, 2, "In the resplendent glow of the cerulean twilight, she contemplated the ephemeral nature of existence. The ineffable beauty of the cosmos seemed to whisper secrets of the ancients, urging her to embrace the serendipity that life bestowed. Each star, a scintilla of hope, beckoned her toward the infinitesimal mysteries that lay beyond the horizon of her understanding." },
                    { 7, 2, "The dichotomy between epistemology and ontology presents a formidable challenge for those endeavoring to delineate the parameters of human knowledge. As we navigate the labyrinthine corridors of perception and reality, it becomes increasingly imperative to scrutinize the veracity of our beliefs. Only through rigorous dialectics can one aspire to transcend the ephemeral nature of subjective experience and approach an authentic comprehension of existence." },
                    { 8, 2, "In the realm of astrophysics, the phenomenon of black holes epitomizes the quintessence of the universe’s enigmatic nature. These celestial behemoths possess gravitational fields so potent that not even light can elude their grasp, rendering them imperceptible to direct observation. The implications of their existence challenge our fundamental understanding of time and space, necessitating a paradigm shift in our approach to cosmological studies." },
                    { 9, 2, "The ramifications of the Industrial Revolution were far-reaching, catalyzing a profound metamorphosis in societal structures. Urbanization burgeoned as agrarian communities dissipated, leading to an unprecedented influx of individuals into burgeoning metropolises. This epoch was characterized by both technological advancements and the concomitant emergence of socio-economic disparities, fostering a milieu of discontent that would later precipitate calls for reform and revolution." },
                    { 10, 2, "The painter's oeuvre is a veritable tapestry of emotive expression, interweaving vibrant hues with intricate brushwork. Each canvas serves as a conduit for the artist's innermost contemplations, reflecting a juxtaposition of the sublime and the grotesque. Through a meticulous examination of form and color, one can discern the underlying ethos that drives the narrative of human experience, compelling viewers to confront their own existential dilemmas." },
                    { 11, 3, "In Lithuanian culture, tradicijos play a vital role in the community. People often gather to celebrate events like Jono Naktis, which showcases their respect for the seasons and nature. This celebration is a perfect example of how traditions unite people and strengthen their identity." },
                    { 12, 3, "When contemplating existence, many ask, \"Koks yra tikrasis zmogaus prigimtis?\" This question delves into the essence of humanity, exploring whether we are mere physical beings or if we possess a deeper, spiritual side. In Lithuania, philosophers discuss the intersection of morality and ethics, which remains relevant globally." },
                    { 13, 3, "Climate change is a pressing global issue, and Lithuania is no exception. Scientists are researching how this phenomenon affects agriculture and biodiversity. They found that ekstremalios oro salygos pose a threat to food security and the economy, emphasizing the need for urgent action." },
                    { 14, 3, "Lithuanian authors often reflect social and political changes in their works. By employing metaphors and simboliai, they convey complex ideas about identity and freedom. Critics argue that such literature not only enriches the cultural landscape but also stimulates discussions about what true freedom means and how it can be achieved." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Words",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
