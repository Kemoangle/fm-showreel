import { Category } from '@/model/category';
import { IPlaylist, IVideos } from '@/model/generatorPlaylist';
import { Restriction } from '@/model/restriction';
import _ from 'lodash';

const getNoBackToBack = (noBackToBack: Category[] | undefined) => {
    if (noBackToBack) {
        const arrText = noBackToBack.map((x) => 'no back to back with ' + x.name);
        return arrText.join('|');
    }
    return '';
};
export class generatorPlaylist {
    createVideo = (data: IVideos) => {
        const newArr = [];
        for (let i = 0; i < data.loop; i++) {
            newArr.push(data);
        }
        return newArr;
    };

    checkCategoriesCloselyTogether = (listVideo: IVideos[] | IPlaylist[]) => {
        const indexVideo = listVideo.findIndex((x, i) => {
            if (i == listVideo.length - 1) {
                return (
                    !this.checkDissimilarCategories(x?.category, listVideo[0]?.category) &&
                    x?.category &&
                    listVideo[0]?.category
                );
            } else {
                return (
                    !this.checkDissimilarCategories(x?.category, listVideo[i + 1]?.category) &&
                    x?.category &&
                    listVideo[i + 1]?.category
                );
            }
        });
        if (indexVideo >= 0) {
            return true;
        }
        return false;
    };

    checkNoBackToBack = (listVideo: IVideos[]) => {
        for (let i = 0; i < listVideo.length; i++) {
            const element = listVideo[i];
            const rule = getNoBackToBack(element?.noBackToBack);
            if (rule) {
                if (rule.toLocaleLowerCase().includes('no back to back with ')) {
                    const btbName = rule.toLocaleLowerCase().split(' with ')[1];

                    if (
                        listVideo[i - 1]?.category?.some((x) =>
                            x.name?.toLocaleLowerCase()?.includes(btbName)
                        ) ||
                        listVideo[i + 1]?.category?.some((x) =>
                            x.name?.toLocaleLowerCase()?.includes(btbName)
                        )
                    ) {
                        return {
                            status: true,
                            index: i,
                            indexReject: listVideo[i - 1]?.category?.some((x) =>
                                x.name?.toLocaleLowerCase()?.includes(btbName)
                            )
                                ? i - 1
                                : i + 1,
                        };
                    }
                }
            }
        }
        return { status: false, index: 0 };
    };

    handleCategoriesCloselyTogether = (listVideo: IVideos[]) => {
        const indexVideo = listVideo.findIndex((x, i) => {
            if (i == listVideo.length - 1) {
                return (
                    !this.checkDissimilarCategories(x?.category, listVideo[0]?.category) &&
                    x?.category &&
                    listVideo[0]?.category
                );
            } else {
                return (
                    !this.checkDissimilarCategories(x?.category, listVideo[i + 1]?.category) &&
                    x?.category &&
                    listVideo[i + 1]?.category
                );
            }
        });

        if (indexVideo >= 0) {
            const indexSwap = listVideo.findIndex((x, i) => {
                if (i == 0) {
                    return (
                        this.checkDissimilarCategories(
                            x?.category,
                            listVideo[indexVideo]?.category
                        ) &&
                        this.checkDissimilarCategories(
                            listVideo[indexVideo]?.category,
                            listVideo[i + 1]?.category
                        ) &&
                        this.checkDissimilarCategories(
                            listVideo[indexVideo]?.category,
                            listVideo[listVideo.length - 1]?.category
                        )
                    );
                } else {
                    return (
                        this.checkDissimilarCategories(
                            x?.category,
                            listVideo[indexVideo]?.category
                        ) &&
                        this.checkDissimilarCategories(
                            listVideo[indexVideo]?.category,
                            listVideo[i + 1]?.category
                        ) &&
                        this.checkDissimilarCategories(
                            listVideo[indexVideo]?.category,
                            listVideo[i - 1]?.category
                        )
                    );
                }
            });

            const newListVideo = [...listVideo];
            if (indexSwap >= 0) {
                let swapVideo: IVideos = _.clone(newListVideo[indexVideo]);
                newListVideo[indexVideo] = _.clone(newListVideo[indexSwap]);
                newListVideo[indexSwap] = _.clone(swapVideo);
            } else {
                if (indexVideo) {
                    let swapVideo: IVideos = newListVideo[indexVideo];
                    newListVideo[indexVideo] = newListVideo[indexVideo - 1];
                    newListVideo[indexVideo - 1] = swapVideo;
                }
            }

            return newListVideo;
        }
        return listVideo;
    };

    checkDissimilarCategories = (
        category1: Category[] | undefined,
        category2: Category[] | undefined
    ) => {
        return !category1?.some((c) => category2?.some((s) => s.name == c.name));
    };

    handleBackToBack = (
        listVideo: IVideos[],
        indexVideo: number,
        indexReject: number | undefined
    ) => {
        const newListVideo = [...listVideo];
        if (indexReject && indexVideo) {
            const indexSwap = listVideo.findIndex((x, i) => {
                if (i == 0) {
                    return (
                        this.checkDissimilarCategories(
                            x?.category,
                            listVideo[indexReject]?.category
                        ) &&
                        this.checkDissimilarCategories(
                            listVideo[indexReject]?.category,
                            listVideo[i + 1]?.category
                        ) &&
                        this.checkDissimilarCategories(
                            listVideo[indexReject]?.category,
                            listVideo[listVideo?.length - 1]?.category
                        ) &&
                        listVideo[indexReject]?.title !== listVideo[i + 1]?.title &&
                        listVideo[indexReject]?.title !== listVideo[listVideo?.length - 1].title &&
                        i !== indexVideo &&
                        i !== indexVideo + 1 &&
                        i !== indexVideo - 1
                    );
                } else {
                    if (i < listVideo.length - 1) {
                        if (listVideo[indexReject]?.category) {
                            return (
                                this.checkDissimilarCategories(
                                    x?.category,
                                    listVideo[indexReject]?.category
                                ) &&
                                this.checkDissimilarCategories(
                                    listVideo[indexReject]?.category,
                                    listVideo[i + 1]?.category
                                ) &&
                                this.checkDissimilarCategories(
                                    listVideo[indexReject]?.category,
                                    listVideo[i - 1]?.category
                                ) &&
                                listVideo[indexReject]?.title !== listVideo[i - 1]?.title &&
                                i !== indexVideo &&
                                i !== indexVideo + 1 &&
                                i !== indexVideo - 1
                            );
                        } else {
                            return (
                                listVideo[indexReject]?.title !== listVideo[i - 1]?.title &&
                                i !== indexVideo &&
                                i !== indexVideo + 1 &&
                                i !== indexVideo - 1
                            );
                        }
                    }
                    return true;
                }
            });

            if (indexSwap >= -1) {
                const swapVideo: IVideos = _.clone(newListVideo[indexReject]);
                newListVideo[indexReject] = _.clone(newListVideo[indexSwap]);
                newListVideo[indexSwap] = _.clone(swapVideo);

                return newListVideo;
            }
        }
        return listVideo;
    };

    handleCheckAndSortCategoriesCloselyTogether = (listVideo: IVideos[]) => {
        let isCategoriesCloselyTogether = this.checkCategoriesCloselyTogether(listVideo);

        let newListVideo: IVideos[] = [];
        let i = 0;
        do {
            i++;
            newListVideo = this.handleCategoriesCloselyTogether(
                newListVideo.length ? newListVideo : listVideo
            );
            isCategoriesCloselyTogether = this.checkCategoriesCloselyTogether(newListVideo);
        } while (isCategoriesCloselyTogether && i < 20);

        return newListVideo;
    };

    handleCheckAndSortNoBackToBack = (listVideo: IVideos[]) => {
        let valueCheckNoBackToBack = this.checkNoBackToBack(listVideo);

        let newListVideo: IVideos[] = [];
        let i = 0;
        do {
            i++;
            newListVideo = this.handleBackToBack(
                newListVideo.length ? newListVideo : listVideo,
                valueCheckNoBackToBack.index,
                valueCheckNoBackToBack.indexReject
            );

            valueCheckNoBackToBack = this.checkNoBackToBack(newListVideo);
        } while (valueCheckNoBackToBack.status && i < 20);

        return newListVideo;
    };

    handleCustomeVideo = (listVideo: IVideos[]) => {
        const newList: IVideos[] = [];
        listVideo.forEach((video) => {
            newList.push(video);
        });
        return newList;
    };

    handleRemoveVideos = (listVideo: IVideos[]) => {
        const newList: IVideos[] = [];
        listVideo.forEach((video) => {
            if (video.loop && video.loop > 1) {
                newList.push({ ...video, loop: video.loop - 1 });
            }
        });
        return newList;
    };

    addLandLordAds = (listVideo: IVideos[], videos: IVideos[]) => {
        let newListVideo: IVideos[] = [...listVideo];
        const newVideosAds = [...videos];
        const loop = newVideosAds.reduce((prev, currentValue) => prev + currentValue.loop, 0);

        const index = +((listVideo.length + loop) / (loop + 1)).toFixed(0);

        let indexVideoAds = 0;

        for (let i = 0; i < loop; i++) {
            newListVideo.splice(index * (i + 1), 0, videos[indexVideoAds]);
            const loopVideo = newVideosAds[indexVideoAds].loop - 1;
            newVideosAds[indexVideoAds].loop = newVideosAds[indexVideoAds].loop - 1;
            if (loopVideo == 0) {
                indexVideoAds++;
            }
        }
        newListVideo = this.handleCheckAndSortCategoriesCloselyTogether(newListVideo);
        newListVideo = this.handleCheckAndSortNoBackToBack(newListVideo);
        return newListVideo;
    };

    createListVideo = (listVideo: IVideos[]) => {
        let oldListVideo = [...listVideo];

        oldListVideo.sort((a, b) => {
            if (a.loop && b.loop) {
                return b.loop - a.loop;
            }
            return 0;
        });

        let newListVideo = [];
        let i = 0;
        do {
            i++;
            newListVideo.push(...this.handleCustomeVideo(oldListVideo));
            oldListVideo = this.handleRemoveVideos(oldListVideo);
        } while (oldListVideo.length && i < 20);

        newListVideo = this.handleCheckAndSortCategoriesCloselyTogether(newListVideo);
        newListVideo = this.handleCheckAndSortNoBackToBack(newListVideo);

        return newListVideo;
    };

    handleRestrictionBuilding = (restriction: Restriction[], videos: IVideos[]) => {
        console.log('generatorPlaylist  restriction:', restriction);
        console.log('generatorPlaylist  videos:', videos);

        let newVideos: IVideos[] = [];
        if (!_.isEmpty(restriction)) {
            newVideos = videos.filter((x) =>
                restriction.every((r) => {
                    if (r.type == 'Except') {
                        return !(
                            x.category?.some(
                                (c) =>
                                    c.name?.toLocaleLowerCase().trim() ==
                                    r.category?.name?.toLocaleLowerCase().trim()
                            ) &&
                            r.arrCategory?.some((s) => x.subCategory.some((v) => v.name == s.name))
                        );
                    } else if (r.type == 'Exclude') {
                        return !(
                            x.category?.some(
                                (c) =>
                                    c.name?.toLocaleLowerCase().trim() ==
                                    r.category?.name?.toLocaleLowerCase().trim()
                            ) &&
                            !r.arrCategory?.some((s) => x.subCategory.some((v) => v.name == s.name))
                        );
                    } else {
                        return x.category?.some(
                            (c) =>
                                c.name?.toLocaleLowerCase().trim() !=
                                r.category?.name?.toLocaleLowerCase().trim()
                        );
                    }
                })
            );
        }

        return this.createListVideo(newVideos);
    };
}
