function startOnboardingTour() {
    const tour = new Shepherd.Tour({
        useModalOverlay: true,
        defaultStepOptions: {
            cancelIcon: {
                enabled: true,
                label: 'Đóng hướng dẫn'
            },
            classes: 'shepherd-custom',
            scrollTo: { behavior: 'smooth', block: 'center' }
        }
    });

    // Hàm trợ giúp để thêm nhân vật và định dạng chung
    const addMascotToStep = (text) => {
        const mascotImage = '/img/mascot.png';
        return `
            <div class="tour-header">
                <img src="${mascotImage}" alt="Trợ lý ảo" class="tour-mascot" />
                <h4 class="tour-title">Trợ lý AI</h4>
            </div>
            <p class="tour-text">${text}</p>
        `;
    };

    // Bước 1: Lời chào
    tour.addStep({
        title: 'Chào mừng bạn!',
        text: addMascotToStep('Tôi là trợ lý AI, rất vui được đồng hành cùng bạn khám phá thế giới công nghệ. Hãy cùng tôi điểm qua một vài tính năng chính nhé!'),
        buttons: [
            {
                action() { return this.next(); },
                text: 'Bắt đầu nào!'
            }
        ]
    });

    // Bước 2: Hướng dẫn về Bài học
    tour.addStep({
        title: 'Các bài học',
        text: addMascotToStep('Đây là nơi chứa các bài viết và video hướng dẫn sử dụng công nghệ, được trình bày một cách đơn giản và dễ hiểu nhất.'),
        attachTo: {
            element: '#nav-articles', // Trỏ vào ID của mục "Bài hướng dẫn"
            on: 'bottom'
        },
        buttons: [
            {
                action() { return this.back(); },
                classes: 'shepherd-button-secondary',
                text: 'Quay lại'
            },
            {
                action() { return this.next(); },
                text: 'Tiếp theo'
            }
        ]
    });

    // Bước 3: Hướng dẫn về Diễn đàn
    tour.addStep({
        title: 'Diễn đàn trao đổi',
        text: addMascotToStep('Bạn có thắc mắc? Hãy vào đây để đặt câu hỏi và trao đổi với những người dùng khác. Mọi người sẽ cùng nhau học hỏi và tiến bộ.'),
        attachTo: {
            element: '#nav-forum', // Trỏ vào ID của mục "Diễn đàn"
            on: 'bottom'
        },
        buttons: [
            {
                action() { return this.back(); },
                classes: 'shepherd-button-secondary',
                text: 'Quay lại'
            },
            {
                action() { return this.next(); },
                text: 'Tiếp theo'
            }
        ]
    });

    // Bước 4: Hướng dẫn về Chatbox
    tour.addStep({
        title: 'Trò chuyện với AI',
        text: addMascotToStep('Nếu có bất kỳ câu hỏi nào cần giải đáp ngay lập tức, bạn có thể nhấn vào đây để trò chuyện trực tiếp nhé!'),
        attachTo: {
            element: '#chat-toggle',
            on: 'left'
        },
        buttons: [
            {
                action() { return this.back(); },
                classes: 'shepherd-button-secondary',
                text: 'Quay lại'
            },
            {
                action() { return this.next(); },
                text: 'Tiếp theo'
            }
        ]
    });

    // Bước 5: Hướng dẫn về Profile người dùng
    tour.addStep({
        title: 'Góc cá nhân của bạn',
        text: addMascotToStep('Đây là khu vực của riêng bạn. Bạn có thể cập nhật thông tin, xem lại các bài học đã lưu và theo dõi tiến trình học tập của mình.'),
        attachTo: {
            element: '#nav-user-profile', // Trỏ vào ID của dropdown người dùng
            on: 'bottom'
        },
        buttons: [
            {
                action() { return this.back(); },
                classes: 'shepherd-button-secondary',
                text: 'Quay lại'
            },
            {
                action() { return this.complete(); },
                text: 'Hoàn thành'
            }
        ]
    });

    const markTourAsSeen = () => {
        const userId = localStorage.getItem('userId');
        if (userId) {
            const tourSeenKey = `tour_seen_for_user_${userId}`;
            localStorage.setItem(tourSeenKey, 'true');
            console.log(`Tour marked as seen for user ${userId}`);
        }
    };

    // Lắng nghe sự kiện khi người dùng hoàn thành tour
    tour.on('complete', markTourAsSeen);

    // Lắng nghe sự kiện khi người dùng hủy tour giữa chừng
    tour.on('cancel', markTourAsSeen);

    // Bắt đầu tour
    tour.start();
}
