<template>
  <div class="create-survey-container">
    <h2>Yeni Anket Oluştur</h2>

    <!-- Anket Başlığı -->
    <div class="form-group">
      <label for="title">Anket Başlığı</label>
      <input v-model="title" id="title" placeholder="Anket Başlığı" class="input-field" />
    </div>

    <!-- Sorular ve Cevaplar -->
    <div v-for="(question, index) in questions" :key="index" class="question-group">
      <div class="form-group">
        <label :for="'question' + index">Soru {{ index + 1 }}</label>
        <input v-model="question.text" :id="'question' + index" placeholder="Soru metni" class="input-field" />
        <button @click="removeQuestion(index)" class="remove-btn">Sil</button>
      </div>

      <div v-for="(option, optIndex) in question.options" :key="optIndex" class="option-group">
        <div class="form-group">
          <label :for="'option' + optIndex">Seçenek {{ optIndex + 1 }}</label>
          <input v-model="option.text" :id="'option' + optIndex" placeholder="Seçenek metni" class="input-field" />
          <button @click="removeOption(index, optIndex)" class="remove-btn">Sil</button>
        </div>
      </div>

      <button @click="addOption(index)" class="add-option-btn">+ Seçenek Ekle</button>
    </div>

    <button @click="addQuestion" class="add-question-btn">+ Soru Ekle</button>
    <button @click="createSurvey" class="submit-btn">Anketi Kaydet</button>

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

        // Sayfa en alta kaydırma işlemi
        this.scrollToBottom();
      },
      removeQuestion(index) {
        this.questions.splice(index, 1);
      },
      addOption(questionIndex) {
        this.questions[questionIndex].options.push({ text: "" });

        // Sayfa en alta kaydırma işlemi
        this.scrollToBottom();
      },
      removeOption(questionIndex, optionIndex) {
        this.questions[questionIndex].options.splice(optionIndex, 1);
      },
      async createSurvey() {
        const requestBody = {
          title: this.title,
          questions: this.questions.map(question => ({
            text: question.text,
            options: question.options.map(option => ({
              text: option.text
            }))
          }))
        };

        console.log('Gönderilen veri:', JSON.stringify(requestBody));

        try {
          const response = await axios.post("http://localhost:5295/api/survey/create", requestBody);
          alert("Anket başarıyla oluşturuldu.");
          this.$router.push("/survey-list");
        } catch (error) {
          console.error("Anket oluşturma hatası:", error);
          alert("Anket oluşturulamadı.");
        }
      },

      // Sayfayı en alta kaydırma fonksiyonu
      scrollToBottom() {
        this.$nextTick(() => {
          const container = document.querySelector('.create-survey-container');
          container.scrollTop = container.scrollHeight;
        });
      }
    }
  };
</script>

<style scoped>
  .create-survey-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 30px;
    background-color: rgba(255, 255, 255, 0.9);
    border-radius: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    font-family: 'Arial', sans-serif;
    color: #333;
    overflow-y: auto; /* Sayfa kaydırma yapılabilmesi için */
    height: 80vh; /* Sayfa yüksekliği */
  }

  h2 {
    text-align: center;
    color: #6a11cb;
    font-size: 2em;
    margin-bottom: 20px;
  }

  .form-group {
    margin-bottom: 20px;
  }

  label {
    font-size: 1.1em;
    margin-bottom: 8px;
    color: #555;
  }

  .input-field {
    width: 100%;
    padding: 10px;
    border-radius: 5px;
    border: 1px solid #ddd;
    margin-bottom: 10px;
    font-size: 1em;
  }

    .input-field:focus {
      border-color: #6a11cb;
      outline: none;
    }

  button {
    padding: 10px 20px;
    background-color: #2575fc;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 1em;
    margin-top: 10px;
  }

    button:hover {
      background-color: #6a11cb;
    }

  .add-question-btn,
  .add-option-btn {
    background-color: #28a745;
  }

  .remove-btn {
    background-color: #dc3545;
  }

  button:focus {
    outline: none;
  }

  /* Anketler Butonu */
  .btn {
    width: 100%;
    padding: 10px;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    margin-top: 20px;
  }

    .btn:hover {
      background-color: #6a11cb;
    }

  .add-option-btn, .add-question-btn {
    background-color: #28a745;
    width: 100%;
  }

    .add-option-btn:hover, .add-question-btn:hover {
      background-color: #218838;
    }

  /* Responsive Design */
  @media (max-width: 768px) {
    .create-survey-container {
      padding: 20px;
    }

    h2 {
      font-size: 1.5em;
    }
  }
</style>
