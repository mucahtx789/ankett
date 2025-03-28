<template>
  <div class="container">
    <h2>Yeni Anket Oluştur</h2>
    <input v-model="title" placeholder="Anket Başlığı" />
    
    <div v-for="(question, index) in questions" :key="index">
      <input v-model="question.text" placeholder="Soru metni" />
      <button @click="removeQuestion(index)">Sil</button>

      <div v-for="(option, optIndex) in question.options" :key="optIndex">
        <input v-model="option.text" placeholder="Seçenek metni" />
        <button @click="removeOption(index, optIndex)">Sil</button>
      </div>
      <button @click="addOption(index)">Seçenek Ekle</button>
    </div>

    <button @click="addQuestion">Soru Ekle</button>
    <button @click="createSurvey">Anketi Kaydet</button>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      title: "",
      questions: [],
    };
  },
  methods: {
    addQuestion() {
      this.questions.push({ text: "", options: [{ text: "" }] });
    },
    removeQuestion(index) {
      this.questions.splice(index, 1);
    },
    addOption(questionIndex) {
      this.questions[questionIndex].options.push({ text: "" });
    },
    removeOption(questionIndex, optionIndex) {
      this.questions[questionIndex].options.splice(optionIndex, 1);
    },
    async createSurvey() {
      try {
        const response = await axios.post("http://localhost:5295/api/survey/create", {
          title: this.title,
          questions: this.questions,
        });

        alert("Anket başarıyla oluşturuldu.");
        this.$router.push("/survey-list");
      } catch (error) {
        console.error("Anket oluşturma hatası:", error);
        alert("Anket oluşturulamadı.");
      }
    },
  },
};
</script>
